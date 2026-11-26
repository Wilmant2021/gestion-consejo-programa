using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class UsuarioService
    {
        private readonly PerfilDbContext _context;

        public UsuarioService(PerfilDbContext context)
        {
            _context = context;
        }

        // Método centralizado para inactivar cualquier usuario (Borrado Lógico)
        public async Task<bool> InactivarUsuarioAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null || usuario.Estado == "Inactivo")
            {
                return false; // No existe o ya se encuentra inactivo
            }

            usuario.Estado = "Inactivo";
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivarUsuarioAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null || usuario.Estado == "Activo")
            {
                return false; // No existe o ya está activo
            }

            usuario.Estado = "Activo";
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<(bool Ok, string Mensaje)> CambiarRolAsync(CambiarRolUsuarioDto dto)
        {
            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.IdUsuario == dto.IdUsuario);

            if (!usuarioExiste)
                return (false, "No se encontró el usuario indicado.");

            var rolExiste = await _context.Roles
                .AnyAsync(r => r.IdRol == dto.IdRol);

            if (!rolExiste)
                return (false, "El rol seleccionado no existe.");

            if (dto.IdPrograma.HasValue)
            {
                var programaExiste = await _context.Programas
                    .AnyAsync(p => p.IdPrograma == dto.IdPrograma.Value);

                if (!programaExiste)
                    return (false, "El programa seleccionado no existe.");
            }

            // Busca la asignación existente del usuario (y del programa si aplica)
            var usuarioRol = await _context.UsuarioRoles
                .FirstOrDefaultAsync(ur =>
                    ur.IdUsuario == dto.IdUsuario &&
                    (!dto.IdPrograma.HasValue || ur.IdPrograma == dto.IdPrograma.Value));

            if (usuarioRol != null)
            {
                if (usuarioRol.IdRol == dto.IdRol)
                    return (false, "El usuario ya tiene asignado ese rol.");

                usuarioRol.IdRol = dto.IdRol;
                if (dto.IdPrograma.HasValue)
                {
                    usuarioRol.IdPrograma = dto.IdPrograma.Value;
                }
            }
            else
            {
                if (!dto.IdPrograma.HasValue)
                    return (false, "Debe indicar un programa para asignar el rol.");

                var nuevoUsuarioRol = new UsuarioRol
                {
                    IdUsuario = dto.IdUsuario,
                    IdRol = dto.IdRol,
                    IdPrograma = dto.IdPrograma.Value
                };

                _context.UsuarioRoles.Add(nuevoUsuarioRol);
            }

            await _context.SaveChangesAsync();
            return (true, "Rol del usuario actualizado exitosamente.");
        }
    }
}