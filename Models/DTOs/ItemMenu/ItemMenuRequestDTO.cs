using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.Enums.ItemMenu;
using System.Text.Json.Serialization;
namespace app_restaurante_backend.Models.DTOs.ItemMenu
{
    public record ItemMenuRequestDTO(
        string Nombre,
        string Descripcion,
        double Precio,
        [property:JsonPropertyName("enlace_imagen")]string EnlaceImagen,
        [property:JsonPropertyName("id_categoria")] short CategoriaId,
        string estado
        )
    {
    }
}
