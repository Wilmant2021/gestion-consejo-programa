using Microsoft.AspNetCore.Mvc;
using Modules.Profile.Application.DOTs;
using Modules.Profile.Application.Services;

namespace Modules.Profile.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AutentificacionService _authService;

        public AuthController(AutentificacionService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var resultado = await _authService.LoginAsync(dto);
                if (resultado == null)
                {
                    return Unauthorized(new { mensaje = "Correo o contraseña incorrectos o usuario no registrado." });
                }

                return Ok(resultado);
            }
            catch (UnauthorizedAccessException ex)
            {
                // <--- AQUÍ CAPTURAMOS CUANDO LA CUENTA ESTÁ INACTIVA
                return StatusCode(403, new { mensaje = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}