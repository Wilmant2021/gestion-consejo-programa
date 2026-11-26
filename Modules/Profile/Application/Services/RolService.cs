using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class RolService
    {
        private readonly PerfilDbContext _context;

        public RolService(PerfilDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolDto>> ObtenerTodosAsync()
        {
            return await _context.Roles
                .Select(r => new RolDto 
                { 
                    Id = r.IdRol, 
                    Nombre = r.RolNombre, 
                    Descripcion = r.Descripcion 
                })
                .ToListAsync();
        }

        public async Task<bool> CrearRolAsync(CrearRolDto dto)
        {
            var existe = await _context.Roles.AnyAsync(r => r.RolNombre == dto.Nombre);
            if (existe) return false;

            var rol = new Rol
            {
                RolNombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return true;
        }

        // NUEVO: Método para eliminar un rol por su ID
        public async Task<bool> EliminarRolAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return false; // El rol no existe
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}