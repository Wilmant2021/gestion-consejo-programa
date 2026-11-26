using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modules.Profile.Application.DTOs
{
    public class RegistrarEstudianteDto : UsuarioBaseDto
    {
        [Required(ErrorMessage = "Debe registrar al menos un programa académico para el estudiante.")]
        public List<EstudianteProgramaDetalleDto> ProgramasAcademicos { get; set; } = new List<EstudianteProgramaDetalleDto>();
    }

    // DTO auxiliar para capturar los datos específicos de cada programa que cursa el estudiante
    public class EstudianteProgramaDetalleDto
    {
        [Required(ErrorMessage = "El ID del programa académico es obligatorio.")]
        public int IdPrograma { get; set; }

        [Required(ErrorMessage = "El código estudiantil es obligatorio.")]
        [StringLength(100, ErrorMessage = "El código estudiantil no puede superar los 100 caracteres.")]
        public string CodigoEstudiantil { get; set; } = string.Empty;

        [Required(ErrorMessage = "La situación académica es obligatoria.")]
        [StringLength(100, ErrorMessage = "La situación académica no puede superar los 100 caracteres.")]
        public string SituacionAcademica { get; set; } = string.Empty;

        [Range(1, 20, ErrorMessage = "El semestre debe estar entre 1 y 20.")]
        public int Semestre { get; set; }

        [Range(0.00, 100.00, ErrorMessage = "El porcentaje debe ser un valor entre 0 y 100.")]
        public decimal PorcentajeCreditosAprobados { get; set; }

        [Required(ErrorMessage = "El plan de estudio es obligatorio.")]
        [StringLength(100, ErrorMessage = "El plan de estudio no puede superar los 100 caracteres.")]
        public string PlanEstudio { get; set; } = string.Empty;
    }
}