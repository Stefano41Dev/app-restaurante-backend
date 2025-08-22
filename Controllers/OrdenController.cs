using app_restaurante_backend.Models.DTOs.Mesa;
using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Service.Implementations;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app_restaurante_backend.Controllers
{
    [Route("api/ordenes")]
    [ApiController]
    [Authorize]

    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _service;

        public OrdenController(IOrdenService Service)
        {
            _service = Service;
        }
        [HttpPost]
        public IActionResult CrearOrden([FromBody] OrdenRequestDto requestDto)
        {
            try
            {
                var orden = _service.CrearOrden(requestDto);
                return Ok(orden);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult obtenerOrdenes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ordenes = _service.ListaOrdenes(pageNumber, pageSize);
            return Ok(ordenes);
        }
        [HttpGet("hoy")]
        public IActionResult obtenerOrdenesDeHoy([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ordenes = _service.ListaOrdenesDeHoy(pageNumber, pageSize);
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public IActionResult obtenerOrden([FromRoute] long id)
        {
            var orden = _service.ObtenerOrden(id);
            if (orden == null)
            {
                return NotFound("Orden no encontrada");
            }
            return Ok(orden);
        }
        [HttpPatch("cambiar-estado/{id}")]
        public IActionResult CambiarEstadoOrden([FromRoute] int id, [FromBody] OrdenEstadoRequestDto estado)
        {
            return Ok(_service.ActualizarEstado(id, estado));
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarOrden([FromRoute] long id, [FromBody] OrdenActualizarRequestDTO requestDto)
        {
            try
            {
                var ordenActualizada = _service.ActualizarOrden(id, requestDto);
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
                return Ok("Todas las ordenes han sido desactivadas correctamente.");
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
        public IActionResult MarcarOrdenPagada([FromRoute] long id)
        {
            return Ok(_service.MarcarOrdenPagada(id));
        }
    }
}
