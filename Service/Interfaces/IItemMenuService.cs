using EntityFrameworkPaginateCore;
using app_restaurante_backend.Models.DTOs.ItemMenu;
namespace app_restaurante_backend.Service.Interfaces
{
    public interface IItemMenuService
    {
        Page<ItemMenuResponseDTO> ObtenerItemsMenu(int pageNumber, int pageSize);
        ItemMenuResponseDTO ObtenerItemMenu(int id);
        ItemMenuResponseDTO AgregarItemMenu(ItemMenuRequestDTO itemMenu);
        ItemMenuResponseDTO ActualizarItemMenu(int id, ItemMenuRequestDTO itemMenu);
        void EliminarItemMenu(int id);
        Page<ItemMenuResponseDTO> ObtenerItemsMenuPorNombre(string nombreItemMenu,string nombreCategoria, int pageNumber, int pageSize);
        ItemMenuResponseDTO CambiarEstadoItemMenu(int id, ItemMenuEstadoRequestDTO estado);
        Page<ItemMenuResponseDTO> ObtenerItemsMenuPorCategoria(int categoriaId, int pageNumber, int pageSize);
    }
}
