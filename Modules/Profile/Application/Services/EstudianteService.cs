using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class EstudianteService
    {
        private readonly PerfilDbContext _context;

        public EstudianteService(PerfilDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarEstudianteAsync(RegistrarEstudianteDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                string correoNormalizado = (dto.CorreoInstitucional ?? string.Empty).Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(correoNormalizado))
                {
                    throw new InvalidOperationException("El correo institucional es obligatorio.");
                }

                // 1. Resolver el identificador externo del nuevo usuario (no el del admin en sesión)
                string identityProviderId = !string.IsNullOrWhiteSpace(dto.IdentityProviderId)
                    ? dto.IdentityProviderId.Trim()
                    : $"local-user-{Guid.NewGuid():N}";

                // Validamos por separado si ya existe el correo institucional
                bool existeCorreo = await _context.Usuarios
                    .AnyAsync(u => u.CorreoInstitucional.ToLower() == correoNormalizado);

                if (existeCorreo)
                {
                    throw new InvalidOperationException(
                        $"Ya existe un usuario registrado con el correo institucional '{correoNormalizado}'.");
                }

                // Validamos por separado si ya existe el IdentityProviderId
                bool existeIdentity = await _context.Usuarios
                    .AnyAsync(u => u.IdentityProviderId == identityProviderId);

                if (existeIdentity)
                {
                    throw new InvalidOperationException(
                        $"El IdentityProviderId '{identityProviderId}' ya se encuentra registrado en el sistema local.");
                }

                // 2. Crear el usuario base
                var usuario = new Usuario
                {
                    IdentityProviderId = identityProviderId,
                    Nombre = (dto.Nombre ?? string.Empty).Trim(),
                    SegundoNombre = dto.SegundoNombre?.Trim(),
                    Apellido = (dto.Apellido ?? string.Empty).Trim(),
                    SegundoApellido = dto.SegundoApellido?.Trim(),
                    CorreoInstitucional = correoNormalizado,
                    Estado = string.IsNullOrWhiteSpace(dto.Estado) ? "Activo" : dto.Estado.Trim()
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync(); // Se genera el IdUsuario

                // 3. Asignar los roles con sus programas opcionales a través de la tabla intermedia Usuario_rol
                var rolesVistos = new HashSet<(int RolId, int? ProgramaId)>();
                foreach (var rolDto in dto.RolesAsignados)
                {
                    if (!rolesVistos.Add((rolDto.RolId, rolDto.ProgramaId)))
                    {
                        throw new InvalidOperationException(
                            $"El rol con ID {rolDto.RolId} y programa {rolDto.ProgramaId?.ToString() ?? "global"} está duplicado en la solicitud.");
                    }

                    bool rolExiste = await _context.Roles
                        .AnyAsync(r => r.IdRol == rolDto.RolId);

                    if (!rolExiste)
                    {
                        throw new InvalidOperationException(
                            $"El rol con ID {rolDto.RolId} no existe.");
                    }

                    if (rolDto.ProgramaId.HasValue)
                    {
                        bool programaRolExiste = await _context.Programas
                            .AnyAsync(p => p.IdPrograma == rolDto.ProgramaId.Value);

                        if (!programaRolExiste)
                        {
                            throw new InvalidOperationException(
                                $"El programa con ID {rolDto.ProgramaId.Value} asociado al rol no existe.");
                        }
                    }

                    var usuarioRol = new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = rolDto.RolId,
                        IdPrograma = rolDto.ProgramaId
                    };
                    _context.UsuarioRoles.Add(usuarioRol);
                }
                await _context.SaveChangesAsync();

                // 4. Crear el perfil general del Estudiante (vinculado al usuario)
                var estudiante = new Estudiante
                {
                    IdUsuario = usuario.IdUsuario
                };

                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync(); // Se genera el IdEstudiante

                // 5. Registrar los programas académicos del estudiante (Soporta Doble Titulación)
                var programasVistos = new HashSet<int>();
                var codigosVistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var progDto in dto.ProgramasAcademicos)
                {
                    if (!programasVistos.Add(progDto.IdPrograma))
                    {
                        throw new InvalidOperationException(
                            $"El programa con ID {progDto.IdPrograma} está duplicado en la solicitud.");
                    }

                    bool programaExiste = await _context.Programas
                        .AnyAsync(p => p.IdPrograma == progDto.IdPrograma);

                    if (!programaExiste)
                    {
                        throw new InvalidOperationException(
                            $"El programa con ID {progDto.IdPrograma} no existe.");
                    }

                    string codigoNormalizado = (progDto.CodigoEstudiantil ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(codigoNormalizado))
                    {
                        throw new InvalidOperationException(
                            $"El código estudiantil para el programa con ID {progDto.IdPrograma} es obligatorio.");
                    }

                    if (!codigosVistos.Add(codigoNormalizado))
                    {
                        throw new InvalidOperationException(
                            $"El código estudiantil '{codigoNormalizado}' está duplicado en la solicitud.");
                    }

                    bool existeCodigo = await _context.EstudianteProgramas
                        .AnyAsync(ep => ep.CodigoEstudiantil == codigoNormalizado);

                    if (existeCodigo)
                    {
                        throw new InvalidOperationException(
                            $"Ya existe un registro con el código estudiantil '{codigoNormalizado}'.");
                    }

                    var estudiantePrograma = new EstudiantePrograma
                    {
                        IdEstudiante = estudiante.IdEstudiante,
                        IdPrograma = progDto.IdPrograma,
                        Semestre = progDto.Semestre,
                        CodigoEstudiantil = codigoNormalizado,
                        SituacionAcademica = (progDto.SituacionAcademica ?? string.Empty).Trim(),
                        PorcentajeCreditosAprobados = progDto.PorcentajeCreditosAprobados,
                        PlanEstudio = (progDto.PlanEstudio ?? string.Empty).Trim()
                    };

                    _context.EstudianteProgramas.Add(estudiantePrograma);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<(bool Ok, string Mensaje)> ActualizarProgramaEstudianteAsync(ActualizarEstudianteProgramaDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var registro = await _context.EstudianteProgramas
                    .Include(ep => ep.Estudiante)
                    .FirstOrDefaultAsync(ep =>
                        ep.IdEstudiantePrograma == dto.IdEstudiantePrograma &&
                        ep.IdEstudiante == dto.IdEstudiante);

                if (registro == null)
                {
                    return (false, "No se encontró la asignación de programa indicada para ese estudiante.");
                }

                var programaExiste = await _context.Programas
                    .AnyAsync(p => p.IdPrograma == dto.IdPrograma);

                if (!programaExiste)
                {
                    return (false, "El programa seleccionado no existe.");
                }

                bool existeDuplicado = await _context.EstudianteProgramas
                    .AnyAsync(ep =>
                        ep.IdEstudiante == dto.IdEstudiante &&
                        ep.IdPrograma == dto.IdPrograma &&
                        ep.IdEstudiantePrograma != dto.IdEstudiantePrograma);

                if (existeDuplicado)
                {
                    return (false, "El estudiante ya tiene registrado este programa en otra asignación.");
                }

                int programaAnteriorId = registro.IdPrograma;
                bool cambiaPrograma = programaAnteriorId != dto.IdPrograma;

                // Si cambia de programa, exigimos los datos completos del nuevo programa para no arrastrar los del anterior
                if (cambiaPrograma)
                {
                    if (!dto.Semestre.HasValue || dto.Semestre.Value <= 0)
                    {
                        return (false, "Al cambiar de programa debes indicar un semestre válido.");
                    }

                    if (string.IsNullOrWhiteSpace(dto.CodigoEstudiantil))
                    {
                        return (false, "Al cambiar de programa debes indicar el nuevo código estudiantil.");
                    }

                    if (string.IsNullOrWhiteSpace(dto.SituacionAcademica))
                    {
                        return (false, "Al cambiar de programa debes indicar la situación académica del nuevo programa.");
                    }

                    if (!dto.PorcentajeCreditosAprobados.HasValue ||
                        dto.PorcentajeCreditosAprobados.Value < 0 ||
                        dto.PorcentajeCreditosAprobados.Value > 100)
                    {
                        return (false, "Al cambiar de programa debes indicar un porcentaje de créditos aprobados válido (0 a 100).");
                    }

                    if (string.IsNullOrWhiteSpace(dto.PlanEstudio))
                    {
                        return (false, "Al cambiar de programa debes indicar el plan de estudio del nuevo programa.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(dto.CodigoEstudiantil))
                {
                    string codigoNormalizado = dto.CodigoEstudiantil.Trim();
                    bool codigoDuplicado = await _context.EstudianteProgramas
                        .AnyAsync(ep =>
                            ep.CodigoEstudiantil == codigoNormalizado &&
                            ep.IdEstudiantePrograma != dto.IdEstudiantePrograma);

                    if (codigoDuplicado)
                    {
                        return (false, $"El código estudiantil '{codigoNormalizado}' ya está asignado a otro registro.");
                    }

                    registro.CodigoEstudiantil = codigoNormalizado;
                }

                registro.IdPrograma = dto.IdPrograma;

                if (dto.Semestre.HasValue)
                {
                    if (dto.Semestre.Value <= 0)
                    {
                        return (false, "El semestre debe ser mayor que cero.");
                    }
                    registro.Semestre = dto.Semestre.Value;
                }

                if (!string.IsNullOrWhiteSpace(dto.SituacionAcademica))
                {
                    registro.SituacionAcademica = dto.SituacionAcademica.Trim();
                }

                if (dto.PorcentajeCreditosAprobados.HasValue)
                {
                    if (dto.PorcentajeCreditosAprobados.Value < 0 || dto.PorcentajeCreditosAprobados.Value > 100)
                    {
                        return (false, "El porcentaje de créditos aprobados debe estar entre 0 y 100.");
                    }
                    registro.PorcentajeCreditosAprobados = dto.PorcentajeCreditosAprobados.Value;
                }

                if (!string.IsNullOrWhiteSpace(dto.PlanEstudio))
                {
                    registro.PlanEstudio = dto.PlanEstudio.Trim();
                }

                // Sincronizar asignaciones en Usuario_rol vinculadas al programa anterior si el estudiante tiene IdUsuario
                if (cambiaPrograma && registro.Estudiante != null)
                {
                    var rolesDelProgramaAnterior = await _context.UsuarioRoles
                        .Where(ur => ur.IdUsuario == registro.Estudiante.IdUsuario && ur.IdPrograma == programaAnteriorId)
                        .ToListAsync();

                    foreach (var ur in rolesDelProgramaAnterior)
                    {
                        bool yaExisteEnNuevoPrograma = await _context.UsuarioRoles
                            .AnyAsync(x =>
                                x.IdUsuario == ur.IdUsuario &&
                                x.IdRol == ur.IdRol &&
                                x.IdPrograma == dto.IdPrograma &&
                                x.IdUsuarioRol != ur.IdUsuarioRol);

                        if (!yaExisteEnNuevoPrograma)
                        {
                            ur.IdPrograma = dto.IdPrograma;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Información del programa del estudiante actualizada exitosamente.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}