namespace app_restaurante_backend.Models.DTOs.DetalleOrden
{
    public record DetalleOrdenRequestDto(
        int PlatoId,
        int Cantidad
    )
    {}
}
