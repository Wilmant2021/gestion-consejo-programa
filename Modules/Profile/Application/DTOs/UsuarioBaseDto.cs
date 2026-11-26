using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DTOs
{
    public class UsuarioBaseDto
    {
        [StringLength(200, ErrorMessage = "El identificador externo no puede superar los 200 caracteres.")]
        public string? IdentityProviderId { get; set; }

        [Required(ErrorMessage = "Debe asignar al menos un rol al usuario.")]
        [MinLength(1, ErrorMessage = "Debe asignar al menos un rol al usuario.")]
        public List<UsuarioRolAsignacionDto> RolesAsignados { get; set; } = new List<UsuarioRolAsignacionDto>();

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "El segundo nombre no puede superar los 50 caracteres.")]
        public string? SegundoNombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "El segundo apellido no puede superar los 50 caracteres.")]
        public string? SegundoApellido { get; set; }

        [Required(ErrorMessage = "El correo institucional es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo institucional no puede superar los 100 caracteres.")]
        public string CorreoInstitucional { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        public string Estado { get; set; } = "Activo";
    }

    public class UsuarioRolAsignacionDto
    {
        [Required(ErrorMessage = "El ID del rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser válido.")]
        public int RolId { get; set; }

        public int? ProgramaId { get; set; }
    }
}