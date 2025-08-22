using app_restaurante_backend.Models.DTOs.Categoria;
using System.Text.Json.Serialization;

namespace app_restaurante_backend.Models.DTOs.DetalleOrden
{
    public record DetalleOrdenResponseDto(
        long Id,
        [property: JsonPropertyName("nombre_item_menu")]  string NombreItem,
        int Cantidad,
        [property: JsonPropertyName("precio_unitario")]   double PrecioUnitario,
        [property: JsonPropertyName("monto_igv")]double Igv,
        double Subtotal,
        double Total
    )
    {
    }
}
