namespace app_restaurante_backend.Models.DTOs.Mesa
{
    public record MesaRequestDTO(
        string Numero,
        short Capacidad,
        string Estado
        )
    {

    }
}
