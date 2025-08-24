using app_restaurante_backend.Custom;
using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace app_restaurante_backend.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CategoriaController(ICategoriaService categoriaService, IHubContext<NotificationHub> hubContext)
        {
            _categoriaService = categoriaService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult ObtenerCategorias()
        {
            var categorias = _categoriaService.ObtenerCategorias();
            return Ok(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] CategoriaRequestDTO categoriaRequestDTO)
        {
            CategoriaResponseDTO? categoriaCreada = _categoriaService.CrearCategoria(categoriaRequestDTO);
            await _hubContext.Clients.All.SendAsync("ActualizarCategoria", categoriaCreada);
            return Ok(categoriaCreada);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerCategoria([FromRoute] int id)
        {
            return Ok(_categoriaService.ObtenerCategoria((short)id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoria([FromRoute] int id)
        {
            _categoriaService.EliminarCategoria((short)id);
            await _hubContext.Clients.All.SendAsync("EliminarCategoria", id);
            return Ok("Se elimino correctamente la categoria");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoria([FromRoute] short id, [FromBody] CategoriaRequestDTO categoriaRequestDTO)
        {
            CategoriaResponseDTO? categoriaActualizada = _categoriaService.ActualizarCategoria(id, categoriaRequestDTO);
            await _hubContext.Clients.All.SendAsync("ActualizarCategoria", categoriaActualizada);
            return Ok(categoriaActualizada);
        }
        
    }
}
