using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    }
}