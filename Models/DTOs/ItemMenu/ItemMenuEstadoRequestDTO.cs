using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.ItemMenu
{
    public record ItemMenuEstadoRequestDTO(
        [property:JsonPropertyName("estado")]string nombreEstado
    )
    {

    }
}
