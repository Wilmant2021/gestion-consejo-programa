using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.DOTs;
using Modules.Profile.Application.Services;
using Modules.Profile.Domain;
using Plataforma.Modules.Profile.Infrastructure;

namespace Modules.Profile.Application.Services
{
    public class ProgramaService
    {
        private readonly PerfilDbContext _context;

        public ProgramaService(PerfilDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramaDto>> ObtenerTodosAsync()
        {
            return await _context.Programas
                .Select(p => new ProgramaDto 
                { 
                    Id = p.IdPrograma, 
                    Nombre = p.Nombre, 
                    Descripcion = p.Descripcion
                })
                .ToListAsync();
        }

        public async Task<bool> CrearProgramaAsync(CrearProgramaDto dto)
        {
            var existe = await _context.Programas.AnyAsync(p => p.Nombre == dto.Nombre);
            if (existe) return false;

            var programa = new Programa
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
            };

            _context.Programas.Add(programa);
            await _context.SaveChangesAsync();
            return true;
        }

        // NUEVO: Método para eliminar un programa por su ID
        public async Task<bool> EliminarProgramaAsync(int id)
        {
            var programa = await _context.Programas.FindAsync(id);
            if (programa == null)
            {
                return false; // El programa no existe
            }

            _context.Programas.Remove(programa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}