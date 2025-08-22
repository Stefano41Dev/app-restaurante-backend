namespace app_restaurante_backend.Models.DTOs.Usuario
{
    public record UsuarioResponseDTO(
        int Id,
        string Nombre,
        string Apellido,
        string Correo,
        string Rol
     );
}
