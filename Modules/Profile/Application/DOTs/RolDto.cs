using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DOTs
{
    public class CrearRolDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }

    // DTO de respuesta para listar con detalles

    public class RolDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}