using app_restaurante_backend.Custom;
using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs.Usuario;
using app_restaurante_backend.Models.Entidades;
using app_restaurante_backend.Service.Interfaces;
using EntityFrameworkPaginateCore;
using Microsoft.EntityFrameworkCore;
namespace app_restaurante_backend.Service.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DbRestauranteContext _context;
        private readonly Utilidades _utilidades;

        public UsuarioService(DbRestauranteContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }
        public UsuarioResponseDTO ActualizarUsuario(int id, UsuarioRequestDTO usuarioDTORequest)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == id);
            if(usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            usuario.Nombre = usuarioDTORequest.Nombre;
            usuario.Apellido = usuarioDTORequest.Apellido;
            usuario.Correo = usuarioDTORequest.Correo;
            usuario.Clave = _utilidades.EncriptarSHA256(usuarioDTORequest.Clave);
            usuario.Rol = usuarioDTORequest.Rol.ToString();

            _context.Usuarios.Update(usuario);
            if (_context.SaveChanges() > 0)
            {
                return new UsuarioResponseDTO(
                    Id: usuario.Id,
                    Nombre: usuario.Nombre,
                    Apellido: usuario.Apellido,
                    Correo: usuario.Correo,
                    Rol: usuario.Rol
                );
            }
            else
            {
                throw new Exception("No se pudo actualizar el usuario");
            }
        }

        public void EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            _context.Usuarios.Remove(usuario);
            if(_context.SaveChanges() <= 0)
            {
                throw new Exception("No se pudo eliminar el usuario");
            }

        }

        public UsuarioResponseDTO GuardarUsuario(UsuarioRequestDTO usuarioDTORequest)
        {
            Usuario usuario = new()
            {
                Nombre = usuarioDTORequest.Nombre,
                Apellido = usuarioDTORequest.Apellido,
                Correo = usuarioDTORequest.Correo,
                Clave = _utilidades.EncriptarSHA256(usuarioDTORequest.Clave),
                Rol = usuarioDTORequest.Rol.ToString(),
                Activo = true
            };

            _context.Usuarios.AddAsync(usuario);
            if (_context.SaveChanges() >0){
                return new UsuarioResponseDTO(
                    Id: usuario.Id,
                    Nombre: usuario.Nombre,
                    Apellido: usuario.Apellido,
                    Correo: usuario.Correo,
                    Rol: usuario.Rol
                );
            }
            else
            {
                throw new Exception("No se pudo registrar el usuario");
            }
            
        }

        public UsuarioResponseDTO ObtenerUsuarioDtoPorCorreo(string correo)
        {
            var usuario = _context.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Correo == correo);
            if(usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            return new UsuarioResponseDTO(
                Id: usuario.Id,
                Nombre: usuario.Nombre,
                Apellido: usuario.Apellido,
                Correo: usuario.Correo,
                Rol: usuario.Rol
            );
        }

        public UsuarioResponseDTO ObtenerUsuarioDtoPorId(int id)
        {
            var usuario = _context.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return new UsuarioResponseDTO(
                Id: usuario.Id,
                Nombre: usuario.Nombre,
                Apellido: usuario.Apellido,
                Correo: usuario.Correo,
                Rol: usuario.Rol
            );
        }

        public List<UsuarioResponseDTO> ObtenerUsuarios()
        {       
            return _context.Usuarios
                .Select(u => new UsuarioResponseDTO(
                   u.Id,
                   u.Nombre,
                   u.Apellido,
                   u.Correo,
                   u.Rol
                )).ToList(); 
        }
    }
}
