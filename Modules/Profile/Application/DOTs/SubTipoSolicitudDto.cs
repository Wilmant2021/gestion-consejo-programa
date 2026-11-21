using Modules.Profile.Domain;
using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DOTs
{
    public class CrearSubTipoSolicitudDto
    {
        [Required(ErrorMessage = "El nombre del sub-tipo de solicitud es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
        
        [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }
    }

    public class SubTipoSolicitudDto
    {
        public int IdSubTipoSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Propiedad de navegación hacia el TipoSolicitud padre
        public TipoSolicitud? TipoSolicitud { get; set; }
    }
}