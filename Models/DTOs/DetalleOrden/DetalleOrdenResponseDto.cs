using app_restaurante_backend.Models.DTOs.Categoria;

namespace app_restaurante_backend.Models.DTOs.DetalleOrden
{
    public record DetalleOrdenResponseDto(
        long Id,
        string NombreItem,
        CategoriaResponseDTO Categoria,
        int Cantidad,
        double PrecioUnitario,
        double Igv,
        double Subtotal,
        double Total
    )
    {
    }
}
