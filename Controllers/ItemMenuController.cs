using app_restaurante_backend.Models.DTOs.ItemMenu;
using app_restaurante_backend.Service.Implementations;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace app_restaurante_backend.Controllers
{
    [Route("api/items-menu")]
    [ApiController]
    [Authorize]
    public class ItemMenuController : ControllerBase
    {
        private readonly IItemMenuService _itemMenuService;
        public ItemMenuController(IItemMenuService itemMenuService)
        {
            _itemMenuService = itemMenuService;
        }
        [HttpPost]
        public IActionResult AgregarItemMenu([FromBody] ItemMenuRequestDTO itemMenu)
        {
            return Ok(_itemMenuService.AgregarItemMenu(itemMenu));
        }
        [HttpPatch("cambiar-estado/{id}")]
        public IActionResult CambiarEstadoItemMenu([FromRoute] int id,[FromBody] ItemMenuEstadoRequestDTO estado)
        {
            return Ok(_itemMenuService.CambiarEstadoItemMenu(id, estado));
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerItemMenu([FromRoute]int id)
        {
            return Ok(_itemMenuService.ObtenerItemMenu(id));
        }
        [HttpGet]
        public IActionResult ObtenerItemsMenu([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_itemMenuService.ObtenerItemsMenu(pageNumber, pageSize));
        }
        /*
        [HttpGet("buscar-por-categoria/")]
        public IActionResult ObtenerItemsMenuPorCategoria([FromQuery] int idCategoria, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_itemMenuService.ObtenerItemsMenuPorCategoria(idCategoria,pageNumber,pageSize));
        }*/
        [HttpDelete("{id}")]
        public IActionResult EliminarItemMenu([FromRoute] int id)
        {
            _itemMenuService.EliminarItemMenu(id);
            return Ok("Se elimino correctamente el item del menu");
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarItemMenu([FromRoute] int id, [FromBody] ItemMenuRequestDTO itemMenu)
        {
            return Ok(_itemMenuService.ActualizarItemMenu(id, itemMenu));
        }
        [HttpGet("buscar/")]
        public IActionResult ObtenerItemsMenuPorNombre([FromQuery] string nombreItemMenu, [FromQuery] string nombreCategoria, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(_itemMenuService.ObtenerItemsMenuPorNombre(nombreItemMenu,nombreCategoria, pageNumber, pageSize));
        }
    }
}
