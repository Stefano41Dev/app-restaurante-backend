using app_restaurante_backend.Models.DTOs.DetalleOrden;
using app_restaurante_backend.Models.Enums.Ordenes;

namespace app_restaurante_backend.Models.DTOs.Orden
{
    public record OrdenResponseDto(
        long Id,
        string CodigoOrden,
        short MesaId,
        string Estado,
        DateTime FechaCreacion,
        TimeSpan HoraCreacion,
        double MontoSubtotal,
        double MontoTotal,
        List<DetalleOrdenResponseDto> detalles
    )
    {
    }
}
