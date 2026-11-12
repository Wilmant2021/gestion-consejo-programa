using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DOTs
{
    // DTO principal que recibe los datos para crear o procesar un usuario
    public class UsuarioBaseDto
    {
        [Required(ErrorMessage = "Debe asignar al menos un rol al usuario.")]
        public List<UsuarioRolAsignacionDto> RolesAsignados { get; set; } = new List<UsuarioRolAsignacionDto>();

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public string? SegundoNombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        public string? SegundoApellido { get; set; }

        [Required(ErrorMessage = "El correo institucional es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100)]
        public string CorreoInstitucional { get; set; } = string.Empty;

        // Estado inicial de la cuenta (por defecto "Activo")
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        public string Estado { get; set; } = "Activo";
    }

    // DTO auxiliar integrado en el mismo archivo para manejar el rol y su programa opcional
    public class UsuarioRolAsignacionDto
    {
        [Required(ErrorMessage = "El ID del rol es obligatorio.")]
        public int RolId { get; set; }

        // Opcional: Null si es global (ej. Admin), o con valor si está atado a un programa (ej. Estudiante / Coordinador)
        public int? ProgramaId { get; set; }
    }
}