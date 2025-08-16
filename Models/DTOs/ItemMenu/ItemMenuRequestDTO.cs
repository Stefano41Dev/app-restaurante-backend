using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.Enums.ItemMenu;
namespace app_restaurante_backend.Models.DTOs.ItemMenu
{
    public record ItemMenuRequestDTO(
        string Nombre,
        string Descripcion,
        double Precio,
        string EnlaceImagen,
        short CategoriaId,
        string estado
        )
    {
    }
}
