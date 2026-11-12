using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Profile.Domain
{
    [Table("Estudiante")]
    public class Estudiante
    {
        [Key]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }
        
        // Relación de navegación hacia el Usuario
        public Usuario Usuario { get; set; } = null!;

        // Relación con los programas que cursa (soporta doble titulación)
        public ICollection<EstudiantePrograma> EstudianteProgramas { get; set; } = new List<EstudiantePrograma>();
    }
}