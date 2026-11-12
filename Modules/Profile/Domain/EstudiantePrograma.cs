using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.Profile.Domain
{
    [Table("Estudiante_programa")]
    public class EstudiantePrograma
    {
        [Key]
        [Column("id_estudiante_programa")]
        public int IdEstudiantePrograma { get; set; }

        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }
        
        // Relación de navegación hacia el Estudiante
        public Estudiante Estudiante { get; set; } = null!;

        [Column("id_programa")]
        public int IdPrograma { get; set; }
        
        // Relación de navegación hacia el Programa académico
        public Programa Programa { get; set; } = null!;

        [Required]
        [Column("semestre")]
        public int Semestre { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("codigo_estudiantil")]
        public string CodigoEstudiantil { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("situacion_academica")]
        public string SituacionAcademica { get; set; } = string.Empty;

        [Required]
        [Column("porcentaje_creditos_aprobados", TypeName = "decimal(5,2)")]
        public decimal PorcentajeCreditosAprobados { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("plan_estudio")]
        public string PlanEstudio { get; set; } = string.Empty;
    }
}