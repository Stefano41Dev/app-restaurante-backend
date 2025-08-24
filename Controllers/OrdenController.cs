using app_restaurante_backend.Custom;
using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace app_restaurante_backend.Controllers
{
    [Route("api/ordenes")]
    [ApiController]
    [Authorize]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _service;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrdenController(IOrdenService Service, IHubContext<NotificationHub> hubContext)
        {
            _service = Service;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] OrdenRequestDto requestDto)
        {
            try
            {
                OrdenResponseDto? orden = _service.CrearOrden(requestDto);
                await _hubContext.Clients.All.SendAsync("ActualizarOrden", orden);
                return Ok(orden);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerOrdenes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ordenes = _service.ListaOrdenes(pageNumber, pageSize);
            return Ok(ordenes);
        }

        [HttpGet("hoy")]
        public IActionResult ObtenerOrdenesDeHoy(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] List<string>? estado = null
        )
        {
            var ordenes = _service.ListaOrdenesDeHoy(pageNumber, pageSize, estado);
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerOrden([FromRoute] long id)
        {
            OrdenResponseDto? orden = _service.ObtenerOrden(id);
            if (orden == null)
            {
                return NotFound("Orden no encontrada");
            }
            return Ok(orden);
        }

        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> CambiarEstadoOrden([FromRoute] int id, [FromBody] OrdenEstadoRequestDto estado)
        {
            OrdenResponseDto? orden = await _service.ActualizarEstado(id, estado);
            await _hubContext.Clients.All.SendAsync("ActualizarEstadoOrden", orden);
            return Ok(orden);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarOrden([FromRoute] long id, [FromBody] OrdenActualizarRequestDTO requestDto)
        {
            try
            {
                OrdenResponseDto? ordenActualizada = _service.ActualizarOrden(id, requestDto);
                await _hubContext.Clients.All.SendAsync("ActualizarOrden", ordenActualizada);
                return Ok(ordenActualizada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DesactivarOrdenes()
        {
            try
            {
                _service.DesactivarOrdenes();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("por-mesa-pendiente/{idMesa}")]
        public IActionResult ObtenerOrdenPendientePorMesa([FromRoute] short idMesa)
        {
            return Ok(_service.ObtenerOrdenPendientePorMesa(idMesa));
        }

        [HttpPatch("pagar/{id}")]
        public async Task<IActionResult> MarcarOrdenPagada([FromRoute] long id)
        {
            return Ok(await _service.MarcarOrdenPagada(id));
        }

    }
}
