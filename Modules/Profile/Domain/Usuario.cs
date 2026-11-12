using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Profile.Domain
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("identity_provider_id")]
        public string IdentityProviderId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("segundo_nombre")]
        public string? SegundoNombre { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("apellido")]
        public string Apellido { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("segundo_apellido")]
        public string? SegundoApellido { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("correo_institucional")]
        public string CorreoInstitucional { get; set; } = string.Empty;

        // Nuevo atributo de estado de cuenta (ej. "Activo", "Inactivo")
        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Activo";

        // Relación con los roles y los programas asociados (Tabla Usuario_rol)
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();

        // Perfil base de estudiante (vinculado al usuario)
        public Estudiante? Estudiante { get; set; }
    }
}