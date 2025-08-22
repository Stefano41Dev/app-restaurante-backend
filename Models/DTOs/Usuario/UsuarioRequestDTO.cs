using app_restaurante_backend.Models.Enums.Usuario;

namespace app_restaurante_backend.Models.DTOs.Usuario
{
    public record UsuarioRequestDTO(
        string Nombre,
        string Apellido,
        string Correo,
        string Clave,
        RolUsuario Rol
    );
}
