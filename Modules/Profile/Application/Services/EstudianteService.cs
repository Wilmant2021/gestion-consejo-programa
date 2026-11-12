using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DOTs;
using Modules.Profile.Application.Services;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class EstudianteService
    {
        private readonly PerfilDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public EstudianteService(PerfilDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> RegistrarEstudianteAsync(RegistrarEstudianteDto dto)
        {
            // Usamos una transacción para asegurar la atomicidad de todo el registro
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Obtenemos el identificador externo simulado actual (cabecera HTTP o valor por defecto)
                string identityProviderId = _currentUserService.GetCurrentIdentityProviderId();

                // Validamos opcionalmente si ya existe ese ID o correo en la base de datos local
                bool existe = await _context.Usuarios
                    .AnyAsync(u => u.IdentityProviderId == identityProviderId || u.CorreoInstitucional == dto.CorreoInstitucional);

                if (existe)
                {
                    throw new System.InvalidOperationException("El usuario ya se encuentra registrado en el sistema local.");
                }

                // 2. Crear el usuario base usando el IdentityProviderId en lugar de contraseña
                var usuario = new Usuario
                {
                    IdentityProviderId = identityProviderId,
                    Nombre = dto.Nombre,
                    SegundoNombre = dto.SegundoNombre,
                    Apellido = dto.Apellido,
                    SegundoApellido = dto.SegundoApellido,
                    CorreoInstitucional = dto.CorreoInstitucional
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync(); // Se genera el IdUsuario

                // 3. Asignar los roles con sus programas opcionales a través de la tabla intermedia Usuario_rol
                foreach (var rolDto in dto.RolesAsignados)
                {
                    var usuarioRol = new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = rolDto.RolId,
                        IdPrograma = rolDto.ProgramaId // Puede ser null si es un rol global, o tener valor
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
                foreach (var progDto in dto.ProgramasAcademicos)
                {
                    var estudiantePrograma = new EstudiantePrograma
                    {
                        IdEstudiante = estudiante.IdEstudiante,
                        IdPrograma = progDto.IdPrograma,
                        Semestre = progDto.Semestre,
                        CodigoEstudiantil = progDto.CodigoEstudiantil,
                        SituacionAcademica = progDto.SituacionAcademica,
                        PorcentajeCreditosAprobados = progDto.PorcentajeCreditosAprobados,
                        PlanEstudio = progDto.PlanEstudio
                    };

                    _context.EstudianteProgramas.Add(estudiantePrograma);
                }
                await _context.SaveChangesAsync();

                // Si todo sale bien, confirmamos la transacción
                await transaction.CommitAsync();
                return true;
            }
            catch (System.Exception)
            {
                // Si ocurre cualquier error, revertimos todos los cambios en la base de datos
                await transaction.RollbackAsync();
                throw; 
            }
        }
    }
}