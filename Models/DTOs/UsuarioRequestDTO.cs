using app_restaurante_backend.Models.Enums;

namespace app_restaurante_backend.Models.DTOs
{
    public record UsuarioRequestDTO(
        string Nombre,
        string Apellido,
        string Correo,
        string Clave,
        RolUsuario Rol
    );
}
