using app_restaurante_backend.Models.DTOs.DetalleOrden;
using app_restaurante_backend.Models.Enums.Ordenes;
using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.Orden
{
    public record OrdenResponseDto(
        long Id,
        [property:JsonPropertyName("codigo_orden")]string CodigoOrden,
        [property:JsonPropertyName("mesa_id")]short MesaId,
        [property:JsonPropertyName("numero_mesa")]string NumeroMesa,
        [property:JsonPropertyName("estado_orden")]string Estado,
        [property:JsonPropertyName("fecha_creacion")]DateTime FechaCreacion,
        [property:JsonPropertyName("hora_creacion")]TimeSpan HoraCreacion,
        [property:JsonPropertyName("monto_sub_total")]double MontoSubtotal,
        [property:JsonPropertyName("monto_total")]double MontoTotal,
        List<DetalleOrdenResponseDto> detalles
    )
    {
    }
}
