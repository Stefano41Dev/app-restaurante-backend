using app_restaurante_backend.Custom;
using app_restaurante_backend.Models.DTOs.Mesa;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace app_restaurante_backend.Controllers
{
    [Route("api/mesas")]
    [ApiController]
    [Authorize]

    public class MesaController : ControllerBase
    {
        private readonly IMesaService _mesaService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public MesaController(IMesaService mesaService, IHubContext<NotificationHub> hubContext)
        {
            _mesaService = mesaService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult ObtenerMesas()
        {
            var mesas = _mesaService.ObtenerMesas();
            return Ok(mesas);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerMesa([FromRoute] int id)
        {
            var mesa = _mesaService.ObtenerMesa(id);
            if (mesa == null)
            {
                return NotFound("Mesa no encontrada");
            }
            return Ok(mesa);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarMesa([FromBody] MesaRequestDTO mesa)
        {
            MesaResponseDTO? mesaCreada = _mesaService.AgregarMesa(mesa);
            await _hubContext.Clients.All.SendAsync("ActualizarMesa", mesaCreada);
            return Ok(mesaCreada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMesa([FromRoute] int id, [FromBody] MesaRequestDTO mesa)
        {
            MesaResponseDTO? mesaActualizada = _mesaService.ActualizarMesa(id, mesa);
            await _hubContext.Clients.All.SendAsync("ActualizarMesa", mesaActualizada);
            return Ok(mesaActualizada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMesa([FromRoute] int id)
        {
            _mesaService.EliminarMesa(id);
            await _hubContext.Clients.All.SendAsync("EliminarMesa", id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> CambiarEstadoMesa([FromRoute] int id, [FromBody] MesaEstadoRequestDTO estado)
        {
            MesaResponseDTO? estadoMesaActualizado = _mesaService.CambiarEstadoMesa(id, estado);
            await _hubContext.Clients.All.SendAsync("ActualizarEstadoMesa", estadoMesaActualizado);
            return Ok(estadoMesaActualizado);
        }

        [HttpGet("disponibles")]
        public IActionResult ObtenerMesasDisponibles()
        {
            return Ok(_mesaService.ObtenerMesasDisponibles());
        }

        [HttpGet("con-orden-pendiente")]
        public IActionResult ObtenerMesasOrdenPendiente()
        {
            return Ok(_mesaService.ObtenerMesasConOrdenPendiente());
        }

    }
}
