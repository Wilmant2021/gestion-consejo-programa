using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Profile.Domain
{
    [Table("Programa")]
    public class Programa
    {
        [Key]
        [Column("id_programa")]
        public int IdPrograma { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Relación con los roles que están atados a este programa (Ej: Coordinadores de programa)
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();

        // Relación con los estudiantes inscritos en este programa (Soporta doble titulación)
        public ICollection<EstudiantePrograma> EstudianteProgramas { get; set; } = new List<EstudiantePrograma>();
    }
}