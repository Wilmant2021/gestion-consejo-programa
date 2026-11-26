using Microsoft.AspNetCore.Mvc;
using Modules.Profile.Application.DTOs;
using Modules.Profile.Application.Services;

namespace Modules.Profile.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoordinadoresController : ControllerBase
    {
        private readonly CoordinadorService _coordinadorService;

        public CoordinadoresController(CoordinadorService coordinadorService)
        {
            _coordinadorService = coordinadorService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCoordinador([FromBody] UsuarioBaseDto dto)
        {
            try
            {
                var resultado = await _coordinadorService.RegistrarCoordinadorAsync(dto);
                if (resultado)
                {
                    return Ok(new { mensaje = "Coordinador registrado exitosamente." });
                }
                return BadRequest(new { mensaje = "No se pudo registrar el coordinador." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}