using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.DetalleOrden
{
    public record DetalleOrdenRequestDto(
        [property: JsonPropertyName("item_menu_id")] int PlatoId,
        int Cantidad
    )
    {}
}
