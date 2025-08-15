using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using app_restaurante_backend.Service.Interfaces;
namespace app_restaurante_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpGet("/lista")]
        public IActionResult ObtenerCategorias([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var categorias = _categoriaService.ObtenerCategorias(pageNumber, pageSize);
            return Ok(categorias);
        }
    }
}
