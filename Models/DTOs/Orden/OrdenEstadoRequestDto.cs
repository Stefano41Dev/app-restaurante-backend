using app_restaurante_backend.Models.Enums.Ordenes;
using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.Orden
{
    public record OrdenEstadoRequestDto(
           [property: JsonPropertyName("estado_orden")] string EstadoOrden
    ) {}
}
