using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class CoordinadorService
    {
        private readonly PerfilDbContext _context;

        public CoordinadorService(PerfilDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarCoordinadorAsync(UsuarioBaseDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                string correoNormalizado = (dto.CorreoInstitucional ?? string.Empty).Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(correoNormalizado))
                {
                    throw new InvalidOperationException("El correo institucional es obligatorio.");
                }

                // 1. Generar identificador externo único para el nuevo coordinador (evita colisión con el usuario en sesión)
                string identityProviderId = $"local-user-{Guid.NewGuid():N}";

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
                    Estado = "Activo"
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync(); // Se genera el IdUsuario

                // 3. Asignar los roles correspondientes (ej. Rol Coordinador con su respectivo ProgramaId opcional)
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
                        bool programaExiste = await _context.Programas
                            .AnyAsync(p => p.IdPrograma == rolDto.ProgramaId.Value);

                        if (!programaExiste)
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

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}