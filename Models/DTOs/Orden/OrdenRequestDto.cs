using app_restaurante_backend.Models.DTOs.DetalleOrden;

namespace app_restaurante_backend.Models.DTOs.Orden
{
    public record OrdenRequestDto(
        short MesaId,
        List<DetalleOrdenRequestDto> Detalles
    ){}
}
