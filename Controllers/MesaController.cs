using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using app_restaurante_backend.Models.DTOs.Mesa;
using Microsoft.AspNetCore.Mvc;

namespace app_restaurante_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        private readonly IMesaService _mesaService;
        public MesaController(IMesaService mesaService)
        {
         _mesaService = mesaService;
        }
        [HttpGet]
        public IActionResult ObtenerMesas(int pageNumber = 1, int pageSize = 10)
        {
            var mesas = _mesaService.ObtenerMesas(pageNumber, pageSize);
            return Ok(mesas);
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerMesa([FromRoute]int id)
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
            if (mesa == null)
            {
                return BadRequest("Datos de la mesa no válidos");
            }
            var nuevaMesa = _mesaService.AgregarMesa(mesa);
            return CreatedAtAction(nameof(ObtenerMesa), new { id = nuevaMesa.Id }, nuevaMesa);
        }
    }
}
