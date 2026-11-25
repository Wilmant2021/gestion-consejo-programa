using Modules.Requests.Application.DOTs;
using Modules.Requests.Domain;
using Plataforma.Modules.Requests.Infrastructure;    
using Microsoft.EntityFrameworkCore;

namespace Modules.Requests.Application.Services
{
    public class TipoSolicitudService
    {
        private readonly RequestsDbContext _context;

        public TipoSolicitudService(RequestsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoSolicitudDto>> ObtenerTodosAsync()
        {
            return await _context.TipoSolicitudes
                .Select(ts => new TipoSolicitudDto 
                { 
                    IdTipoSolicitud = ts.IdTipoSolicitud,
                    Nombre = ts.Nombre, 
                    Descripcion = ts.Descripcion 
                })
                .ToListAsync();
        }

        public async Task<bool> CrearTipoSolicitudAsync(CrearTipoSolicitudDto dto)
        {
            var existe = await _context.TipoSolicitudes.AnyAsync(ts => ts.Nombre == dto.Nombre);
            if (existe) return false;

            var tipoSolicitud = new TipoSolicitud
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
            };

            _context.TipoSolicitudes.Add(tipoSolicitud);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarTipoSolicitudAsync(int id)
        {
            var tipoSolicitud = await _context.TipoSolicitudes.FindAsync(id);
            if (tipoSolicitud == null)
            {
                return false; // El tipo de solicitud no existe
            }

            _context.TipoSolicitudes.Remove(tipoSolicitud);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}