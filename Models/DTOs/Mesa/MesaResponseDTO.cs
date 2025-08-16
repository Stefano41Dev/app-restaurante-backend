using app_restaurante_backend.Models.Enums.Ordenes;

namespace app_restaurante_backend.Models.DTOs.Mesa
{
    public record MesaResponseDTO(
        short Id,
        string Numero,
        short Capacidad,
        EstadoMesa Estado
    )
    {

    }
}
