using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using app_restaurante_backend.Service.Interfaces;
using app_restaurante_backend.Models.DTOs.Categoria;
using System.Net;
using Microsoft.AspNetCore.Authorization;
namespace app_restaurante_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpGet]
        public IActionResult ObtenerCategorias([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var categorias = _categoriaService.ObtenerCategorias(pageNumber, pageSize);
            return Ok(categorias);
        }
        [HttpPost]
        public IActionResult crearCategoria(CategoriaRequestDTO categoriaRequestDTO)
        {
            return Ok(_categoriaService.CrearCategoria(categoriaRequestDTO));
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerCategoria(int id)
        {
            return Ok(_categoriaService.ObtenerCategoria((short)id));
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarCategoria(int id)
        {
            _categoriaService.EliminarCategoria((short)id);
            return Ok("Se elimino correctamente la categoria");
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarCategoria(short id, CategoriaRequestDTO categoriaRequestDTO)
        {
            return Ok(_categoriaService.ActualizarCategoria(id, categoriaRequestDTO));
        }
    }
}
