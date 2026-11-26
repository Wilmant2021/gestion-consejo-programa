using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DTOs
{
    public class CrearRolDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
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