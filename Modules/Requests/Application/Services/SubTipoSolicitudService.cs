using Microsoft.EntityFrameworkCore;
using Modules.Requests.Application.DOTs;
using Modules.Requests.Domain;
using Plataforma.Modules.Requests.Infrastructure;

namespace Modules.Requests.Application.Services
{
    public class SubTipoSolicitudService
    {
        private readonly RequestsDbContext _context;

        public SubTipoSolicitudService(RequestsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubTipoSolicitudDto>> ObtenerTodosAsync()
        {
            return await _context.SubTipoSolicitudes
                .Select(sts => new SubTipoSolicitudDto
                {
                    IdSubTipoSolicitud = sts.IdSubTipoSolicitud,
                    IdTipoSolicitud = sts.IdTipoSolicitud,
                    NombreTipoSolicitud = sts.TipoSolicitud != null ? sts.TipoSolicitud.Nombre : null,
                    Nombre = sts.Nombre,
                    Descripcion = sts.Descripcion
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SubTipoSolicitudDto>> ObtenerPorIdTipoSolicitudAsync(int idTipoSolicitud)
        {
            return await _context.SubTipoSolicitudes
                .Where(sts => sts.IdTipoSolicitud == idTipoSolicitud)
                .Select(sts => new SubTipoSolicitudDto
                {
                    IdSubTipoSolicitud = sts.IdSubTipoSolicitud,
                    IdTipoSolicitud = sts.IdTipoSolicitud,
                    NombreTipoSolicitud = sts.TipoSolicitud != null ? sts.TipoSolicitud.Nombre : null,
                    Nombre = sts.Nombre,
                    Descripcion = sts.Descripcion
                })
                .ToListAsync();
        }

        public async Task<bool> CrearSubTipoSolicitudAsync(CrearSubTipoSolicitudDto dto)
        {
            var tipoExiste = await _context.TipoSolicitudes
                .AnyAsync(t => t.IdTipoSolicitud == dto.IdTipoSolicitud);

            if (!tipoExiste)
                return false;

            var existe = await _context.SubTipoSolicitudes
                .AnyAsync(sts => sts.IdTipoSolicitud == dto.IdTipoSolicitud && sts.Nombre == dto.Nombre);

            if (existe)
                return false;

            var subTipoSolicitud = new SubTipoSolicitud
            {
                IdTipoSolicitud = dto.IdTipoSolicitud,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.SubTipoSolicitudes.Add(subTipoSolicitud);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarSubTipoSolicitudAsync(int id)
        {
            var subTipoSolicitud = await _context.SubTipoSolicitudes.FindAsync(id);
            if (subTipoSolicitud == null)
            {
                return false;
            }

            _context.SubTipoSolicitudes.Remove(subTipoSolicitud);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}