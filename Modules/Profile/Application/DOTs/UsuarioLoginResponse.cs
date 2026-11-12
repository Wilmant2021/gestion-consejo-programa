using System.Collections.Generic;

namespace Modules.Profile.Application.DOTs
{
    public class UsuarioLoginResponseDto
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoInstitucional { get; set; } = string.Empty;
        
        // Lista de roles y los programas a los que están atados (si aplica)
        public List<RolProgramaResponseDto> RolesYProgramas { get; set; } = new List<RolProgramaResponseDto>();
    }

    public class RolProgramaResponseDto
    {
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public int? ProgramaId { get; set; }
        public string? ProgramaNombre { get; set; }
    }
}