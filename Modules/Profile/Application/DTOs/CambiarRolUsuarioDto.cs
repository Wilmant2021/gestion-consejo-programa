using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DTOs
{
    public class CambiarRolUsuarioDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser válido.")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El ID del nuevo rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser válido.")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El ID del programa es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del programa debe ser válido.")]
        public int? IdPrograma { get; set; }
    }
}