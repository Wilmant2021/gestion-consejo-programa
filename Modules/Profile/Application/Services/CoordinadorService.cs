using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DOTs;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class CoordinadorService
    {
        private readonly PerfilDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CoordinadorService(PerfilDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> RegistrarCoordinadorAsync(UsuarioBaseDto dto)
        {
            // Usamos una transacción para asegurar la atomicidad de la operación
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Obtenemos el identity_provider_id actual (simulado por cabecera HTTP o token externo)
                string currentIdentityProviderId = _currentUserService.GetCurrentIdentityProviderId();

                // 2. Crear el usuario base usando IdentityProviderId en lugar de Contrasena
                var usuario = new Usuario
                {
                    IdentityProviderId = currentIdentityProviderId,
                    Nombre = dto.Nombre,
                    SegundoNombre = dto.SegundoNombre,
                    Apellido = dto.Apellido,
                    SegundoApellido = dto.SegundoApellido,
                    CorreoInstitucional = dto.CorreoInstitucional
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync(); // Se genera el IdUsuario

                // 3. Asignar los roles correspondientes (ej. Rol Coordinador con su respectivo ProgramaId opcional)
                foreach (var rolDto in dto.RolesAsignados)
                {
                    var usuarioRol = new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = rolDto.RolId,
                        IdPrograma = rolDto.ProgramaId // Aquí se asocia el programa si es un coordinador de carrera específica, o null si es general
                    };
                    _context.UsuarioRoles.Add(usuarioRol);
                }

                await _context.SaveChangesAsync();

                // Si todo sale bien, confirmamos la transacción
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                // Si ocurre algún fallo, revertimos todos los cambios
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}