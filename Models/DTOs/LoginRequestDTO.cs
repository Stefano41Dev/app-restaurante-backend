namespace app_restaurante_backend.Models.DTOs
{
    public record LoginRequestDTO(
        string Correo,
        string Clave
    );
}
