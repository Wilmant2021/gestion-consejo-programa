using Microsoft.AspNetCore.Mvc;
using Modules.Requests.Application.DOTs;
using Modules.Requests.Application.Services;

namespace Modules.Requests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposSolicitudController : ControllerBase
    {
        private readonly TipoSolicitudService _tipoSolicitudService;

        public TiposSolicitudController(TipoSolicitudService tipoSolicitudService)
        {
            _tipoSolicitudService = tipoSolicitudService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var lista = await _tipoSolicitudService.ObtenerTodosAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al consultar los tipos de solicitud.", detalle = ex.Message });
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] CrearTipoSolicitudDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creado = await _tipoSolicitudService.CrearTipoSolicitudAsync(dto);
                if (!creado)
                {
                    return BadRequest(new { mensaje = "No se pudo crear el tipo de solicitud. Verifica si ya existe." });
                }

                return Ok(new { mensaje = "Tipo de solicitud creado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al crear el tipo de solicitud.", detalle = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _tipoSolicitudService.EliminarTipoSolicitudAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = "No se encontró el tipo de solicitud." });
                }

                return Ok(new { mensaje = "Tipo de solicitud eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al eliminar el tipo de solicitud.", detalle = ex.Message });
            }
        }
    }
}