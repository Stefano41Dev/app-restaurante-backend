using Microsoft.AspNetCore.Http;
using app_restaurante_backend.Service.Interfaces;
using app_restaurante_backend.Models.DTOs.Orden;
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
    }
}
