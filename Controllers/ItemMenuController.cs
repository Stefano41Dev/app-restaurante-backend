using app_restaurante_backend.Custom;
using app_restaurante_backend.Models.DTOs.ItemMenu;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace app_restaurante_backend.Controllers
{
    [Route("api/items-menu")]
    [ApiController]
    [Authorize]
    public class ItemMenuController : ControllerBase
    {

        private readonly IItemMenuService _itemMenuService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ItemMenuController(IItemMenuService itemMenuService, IHubContext<NotificationHub> hubContext)
        {
            _itemMenuService = itemMenuService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarItemMenu([FromBody] ItemMenuRequestDTO itemMenu)
        {
            ItemMenuResponseDTO? nuevoItem = _itemMenuService.AgregarItemMenu(itemMenu);
            await _hubContext.Clients.All.SendAsync("NuevoItemMenu", nuevoItem);
            return Ok(nuevoItem);
        }

        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> CambiarEstadoItemMenu(
            [FromRoute] int id,
            [FromBody] ItemMenuEstadoRequestDTO estado
        )
        {
            ItemMenuResponseDTO? cambioEstadoItem = _itemMenuService.CambiarEstadoItemMenu(id, estado);
            await _hubContext.Clients.All.SendAsync("CambioEstadoItemMenu", cambioEstadoItem);
            return Ok(cambioEstadoItem);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerItemMenu([FromRoute]int id)
        {
            return Ok(_itemMenuService.ObtenerItemMenu(id));
        }

        [HttpGet]
        public IActionResult ObtenerItemsMenu(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            return Ok(_itemMenuService.ObtenerItemsMenu(pageNumber, pageSize));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarItemMenu([FromRoute] int id)
        {
            _itemMenuService.EliminarItemMenu(id);
            await _hubContext.Clients.All.SendAsync("EliminarItemMenu", id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarItemMenu(
            [FromRoute] int id,
            [FromBody] ItemMenuRequestDTO itemMenu
        )
        {
            ItemMenuResponseDTO? itemMenuActualizado = _itemMenuService.ActualizarItemMenu(id, itemMenu);
            await _hubContext.Clients.All.SendAsync("ActualizarItemMenu", itemMenuActualizado);
            return Ok(itemMenuActualizado);
        }

        [HttpGet("buscar/")]
        public IActionResult ObtenerItemsMenuPorNombre(
            [FromQuery(Name = "nombre")] string nombreItemMenu = "",
            [FromQuery(Name = "categoria")] string nombreCategoria = "",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            return Ok(_itemMenuService.ObtenerItemsMenuPorNombre(nombreItemMenu,nombreCategoria, pageNumber, pageSize));
        }

    }
}
