using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.Categoria
{
    public record CategoriaRequestDTO(
        string Nombre,
        string Descripcion,
        [property: JsonPropertyName("precio_minimo")]  double PrecioMinimo
    )
    {
        
    }
}
