using app_restaurante_backend.Models.DTOs.Usuario;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Interfaces
{
    public interface IUsuarioService
    {
        Page<UsuarioResponseDTO> ObtenerUsuarios(int numeroPagina, int cantidadElementos);
        UsuarioResponseDTO ObtenerUsuarioDtoPorCorreo(string correo);
        UsuarioResponseDTO ObtenerUsuarioDtoPorId(int id);
        UsuarioResponseDTO GuardarUsuario(UsuarioRequestDTO usuarioDTORequest);
        UsuarioResponseDTO ActualizarUsuario(int id, UsuarioRequestDTO usuarioDTORequest);
        void EliminarUsuario(int id);
    }
}
