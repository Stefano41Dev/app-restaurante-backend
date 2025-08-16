using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.Enums.ItemMenu;

namespace app_restaurante_backend.Models.DTOs.ItemMenu
{
    public record ItemMenuResponseDTO(
        int Id,
        string Nombre,
        string Descripcion,
        double Precio,
        string EnlaceImagen,
        CategoriaResponseDTO Categoria,
        EstadoItemMenu Estado
        )
    {
    }
}
