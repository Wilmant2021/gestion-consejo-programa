using System.ComponentModel.DataAnnotations;

namespace Modules.Requests.Application.DOTs
{
    public class CrearSubTipoSolicitudDto
    {
        [Required(ErrorMessage = "El tipo de solicitud es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de solicitud válido.")]
        public int IdTipoSolicitud { get; set; }

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
        public string? NombreTipoSolicitud { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}