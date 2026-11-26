using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DTOs
{
    public class ActualizarEstudianteProgramaDto
    {
        [Required(ErrorMessage = "El ID del registro estudiante-programa es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del registro estudiante-programa debe ser válido.")]
        public int IdEstudiantePrograma { get; set; }

        [Required(ErrorMessage = "El ID del estudiante es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del estudiante debe ser válido.")]
        public int IdEstudiante { get; set; }

        [Required(ErrorMessage = "El ID del programa es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del programa debe ser válido.")]
        public int IdPrograma { get; set; }

        [Range(1, 30, ErrorMessage = "El semestre debe ser válido.")]
        public int? Semestre { get; set; }

        [StringLength(100)]
        public string? CodigoEstudiantil { get; set; }

        [StringLength(100)]
        public string? SituacionAcademica { get; set; }

        [Range(0, 100)]
        public decimal? PorcentajeCreditosAprobados { get; set; }

        [StringLength(100)]
        public string? PlanEstudio { get; set; }
    }
}