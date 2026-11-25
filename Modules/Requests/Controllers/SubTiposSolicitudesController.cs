using Microsoft.AspNetCore.Mvc;
using Modules.Requests.Application.DOTs;
using Modules.Requests.Application.Services;

namespace Modules.Requests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTipoSolicitudController : ControllerBase
    {
        private readonly SubTipoSolicitudService _subTipoSolicitudService;

        public SubTipoSolicitudController(SubTipoSolicitudService subTipoSolicitudService)
        {
            _subTipoSolicitudService = subTipoSolicitudService;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int? idTipoSolicitud = null)
        {
            try
            {
                var lista = idTipoSolicitud.HasValue
                    ? await _subTipoSolicitudService.ObtenerPorIdTipoSolicitudAsync(idTipoSolicitud.Value)
                    : await _subTipoSolicitudService.ObtenerTodosAsync();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al consultar los subtipos de solicitud.", detalle = ex.Message });
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] CrearSubTipoSolicitudDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creado = await _subTipoSolicitudService.CrearSubTipoSolicitudAsync(dto);
                if (!creado)
                {
                    return BadRequest(new { mensaje = "No se pudo crear el subtipo de solicitud. Verifica que el tipo exista y que el nombre no esté repetido." });
                }

                return Ok(new { mensaje = "Subtipo de solicitud creado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al crear el subtipo de solicitud.", detalle = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _subTipoSolicitudService.EliminarSubTipoSolicitudAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = "No se encontró el subtipo de solicitud." });
                }

                return Ok(new { mensaje = "Subtipo de solicitud eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al eliminar el subtipo de solicitud.", detalle = ex.Message });
            }
        }
    }
}