using System.ComponentModel.DataAnnotations;

namespace Modules.Requests.Application.DOTs
{
    public class CrearTipoSolicitudDto
    {
        [Required(ErrorMessage = "El nombre del tipo de solicitud es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }
    }

    public class TipoSolicitudDto
    {
        public int IdTipoSolicitud { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}