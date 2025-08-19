using app_restaurante_backend.Models.DTOs.Mesa;
using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Service.Implementations;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app_restaurante_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult obtenerOrdenes(int pageNumber = 1, int pageSize = 10)
        {
            var ordenes = _service.ListaOrdenes(pageNumber, pageSize);
            return Ok(ordenes);
        }
        [HttpGet("{id}")]
        public IActionResult obtenerOrden([FromRoute]long id)
        {
            var orden = _service.ObtenerOrden(id);
            if (orden == null)
            {
                return NotFound("Orden no encontrada");
            }
            return Ok(orden);
        }
        [HttpPatch("{id}")]
        public IActionResult CambiarEstadoOrden([FromRoute] int id, [FromBody] OrdenEstadoRequestDto estado)
        {
            return Ok(_service.ActualizarEstado(id,estado));
        }
    }
}
