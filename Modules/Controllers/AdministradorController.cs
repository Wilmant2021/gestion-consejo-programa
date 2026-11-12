using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Profile.Application.DOTs;
using Modules.Profile.Application.Services;

namespace gestionConsejoProgramaWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradoresController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly ProgramaService _programaService;
        private readonly RolService _rolService;

        public AdministradoresController(
            UsuarioService usuarioService,
            ProgramaService programaService,
            RolService rolService)
        {
            _usuarioService = usuarioService;
            _programaService = programaService;
            _rolService = rolService;
        }

        // --- GESTIÓN DE ESTADOS DE USUARIO ---

        [HttpPatch("usuarios/{id}/inactivar")]
        public async Task<IActionResult> InactivarUsuario(int id)
        {
            try
            {
                var resultado = await _usuarioService.InactivarUsuarioAsync(id);

                if (!resultado)
                {
                    return NotFound(new { mensaje = "El usuario no fue encontrado o ya se encuentra inactivo." });
                }

                return Ok(new { mensaje = "Cuenta desactivada correctamente (borrado lógico aplicado)." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPatch("usuarios/{id}/activar")]
        public async Task<IActionResult> ActivarUsuario(int id)
        {
            try
            {
                var resultado = await _usuarioService.ActivarUsuarioAsync(id);

                if (!resultado)
                {
                    return NotFound(new { mensaje = "El usuario no fue encontrado o ya se encuentra activo." });
                }

                return Ok(new { mensaje = "Cuenta activada exitosamente." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        // --- GESTIÓN DE PROGRAMAS ---

        [HttpGet("programas")]
        public async Task<IActionResult> ListarProgramas()
        {
            try
            {
                var programas = await _programaService.ObtenerTodosAsync();
                return Ok(programas);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("programas")]
        public async Task<IActionResult> CrearPrograma([FromBody] CrearProgramaDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var resultado = await _programaService.CrearProgramaAsync(dto);
                if (!resultado)
                {
                    return BadRequest(new { mensaje = "El programa ya existe o no pudo ser creado." });
                }

                return Ok(new { mensaje = "Programa creado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        // NUEVO: Endpoint para eliminar un programa
        [HttpDelete("programas/{id}")]
        public async Task<IActionResult> EliminarPrograma(int id)
        {
            try
            {
                var resultado = await _programaService.EliminarProgramaAsync(id);
                if (!resultado)
                {
                    return NotFound(new { mensaje = "El programa no fue encontrado o no pudo ser eliminado." });
                }

                return Ok(new { mensaje = "Programa eliminado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        // --- GESTIÓN DE ROLES ---

        [HttpGet("roles")]
        public async Task<IActionResult> ListarRoles()
        {
            try
            {
                var roles = await _rolService.ObtenerTodosAsync();
                return Ok(roles);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CrearRol([FromBody] CrearRolDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var resultado = await _rolService.CrearRolAsync(dto);
                if (!resultado)
                {
                    return BadRequest(new { mensaje = "El rol ya existe o no pudo ser creado." });
                }

                return Ok(new { mensaje = "Rol creado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }

        // NUEVO: Endpoint para eliminar un rol
        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> EliminarRol(int id)
        {
            try
            {
                var resultado = await _rolService.EliminarRolAsync(id);
                if (!resultado)
                {
                    return NotFound(new { mensaje = "El rol no fue encontrado o no pudo ser eliminado." });
                }

                return Ok(new { mensaje = "Rol eliminado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}