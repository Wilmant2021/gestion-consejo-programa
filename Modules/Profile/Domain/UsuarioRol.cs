using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Profile.Domain
{
    [Table("Usuario_rol")]
    public class UsuarioRol
    {
        [Key]
        [Column("id_usuario_rol")]
        public int IdUsuarioRol { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        [Column("id_rol")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!;

        // Opcional / Nullable: Es null si el rol es global (ej. Administrador), o tiene valor si está atado a un programa específico (ej. Coordinador)
        [Column("id_programa")]
        public int? IdPrograma { get; set; }

        [ForeignKey("IdPrograma")]
        public Programa? Programa { get; set; }
    }
}