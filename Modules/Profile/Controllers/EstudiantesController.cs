using Microsoft.AspNetCore.Mvc;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Application.Services;

namespace Modules.Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController: ControllerBase
    {
        private readonly EstudianteService _estudianteService;

        public EstudiantesController(EstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarEstudianteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _estudianteService.RegistrarEstudianteAsync(dto);
                return Ok(new { mensaje = "Estudiante registrado exitosamente en Azure SQL." });
            }
            catch (Exception ex)
            {
                // En un entorno real es mejor registrar el error con un ILogger
                return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el estudiante.", detalle = ex.Message });
            }
        }
    }
    
}