using app_restaurante_backend.Models.DTOs.Mesa;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app_restaurante_backend.Controllers
{
    [Route("api/mesas")]
    [ApiController]
    [Authorize]

    public class MesaController : ControllerBase
    {
        private readonly IMesaService _mesaService;
        public MesaController(IMesaService mesaService)
        {
            _mesaService = mesaService;
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
        public IActionResult AgregarMesa([FromBody] MesaRequestDTO mesa)
        {
            return Ok(_mesaService.AgregarMesa(mesa));
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarMesa([FromRoute] int id, [FromBody] MesaRequestDTO mesa)
        {
            return Ok(_mesaService.ActualizarMesa(id, mesa));
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarMesa([FromRoute] int id)
        {
            _mesaService.EliminarMesa(id);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult CambiarEstadoMesa([FromRoute] int id, [FromBody] MesaEstadoRequestDTO estado)
        {
            return Ok(_mesaService.CambiarEstadoMesa(id, estado));
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
