using app_restaurante_backend.Custom;
using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs;
using app_restaurante_backend.Models.DTOs.Usuario;
using app_restaurante_backend.Models.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app_restaurante_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccesoController : ControllerBase
    {

        private readonly DbRestauranteContext _context;
        private readonly Utilidades _utilidades;

        public AccesoController(DbRestauranteContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }
        
        /*
        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse([FromBody] UsuarioRequestDTO usuarioDto)
        {

            Usuario usuario = new()
            {
                Nombre = usuarioDto.Nombre,
                Apellido = usuarioDto.Apellido,
                Correo = usuarioDto.Correo,
                Clave = _utilidades.EncriptarSHA256(usuarioDto.Clave),
                Rol = usuarioDto.Rol.ToString(),
                Activo = true
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            if (usuario.Id != 0) { return Ok(new { isSuccess = true, message = "Usuario registrado correctamamente" }); }
            else { return BadRequest(new { isSuccess = false, message = "No se pudo registrar el usuario" }); }

        }*/

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginRequestDTO loginDto)
        {
            Usuario? usuarioEncontrado = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == loginDto.Correo && u.Clave == _utilidades.EncriptarSHA256(loginDto.Clave) && u.Activo == true);

            if (usuarioEncontrado == null) { return Unauthorized(new { isSuccess = false, message = "Credenciales incorrectas" }); }
            else { return Ok(new { isSuccess = true, token = _utilidades.GenerarJWT(usuarioEncontrado) }); }
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public IActionResult CerrarSesion()
        {
            return NoContent();
        }

    }
}
