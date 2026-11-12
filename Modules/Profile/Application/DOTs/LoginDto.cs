using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DOTs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El correo institucional es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoInstitucional { get; set; } = string.Empty;
    }
}