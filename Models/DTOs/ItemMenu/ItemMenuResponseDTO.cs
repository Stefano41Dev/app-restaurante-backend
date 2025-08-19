using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.Enums.ItemMenu;
using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.ItemMenu
{
    public record ItemMenuResponseDTO(
        int Id,
        string Nombre,
        string Descripcion,
        double Precio,
        [property:JsonPropertyName("enlace_imagen")]string EnlaceImagen,
        CategoriaResponseDTO Categoria,
        string Estado
        )
    {
    }
}
