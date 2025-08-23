using app_restaurante_backend.Models.DTOs.Usuario;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Interfaces
{
    public interface IUsuarioService
    {
        List<UsuarioResponseDTO> ObtenerUsuarios();
        UsuarioResponseDTO ObtenerUsuarioDtoPorCorreo(string correo);
        UsuarioResponseDTO ObtenerUsuarioDtoPorId(int id);
        UsuarioResponseDTO GuardarUsuario(UsuarioRequestDTO usuarioDTORequest);
        UsuarioResponseDTO ActualizarUsuario(int id, UsuarioRequestDTO usuarioDTORequest);
        void EliminarUsuario(int id);
    }
}
