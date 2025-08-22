using app_restaurante_backend.Models.DTOs.Usuario;
using app_restaurante_backend.Service.Implementations;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
namespace app_restaurante_backend.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult GuardarUsuario([FromBody] UsuarioRequestDTO usuarioDTORequest)
        {
            try
            {
                UsuarioResponseDTO usuarioResponse = _service.GuardarUsuario(usuarioDTORequest);
                return Ok(new { isSuccess = true, message = "Usuario registrado correctamente", data = usuarioResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }
        [HttpGet("me")]
        public ActionResult ObtenerUsuarioAutenticado()
        {
          
            var userClaims = HttpContext.User;

            
            var userEmail = userClaims.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
               
                return BadRequest("El token de autenticación no contiene el correo electrónico.");
            }

            var usuarioDtoResponse = _service.ObtenerUsuarioDtoPorCorreo(userEmail);

            if (usuarioDtoResponse == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(usuarioDtoResponse);
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarUsuario([FromRoute]int id, [FromBody] UsuarioRequestDTO usuarioDTORequest)
        {
            try
            {
                UsuarioResponseDTO usuarioResponse = _service.ActualizarUsuario(id, usuarioDTORequest);
                return Ok(new { isSuccess = true, message = "Usuario actualizado correctamente", data = usuarioResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult ObtenerUsuarios([FromQuery]int pageNumber = 1,[FromQuery] int pageSize =10)
        {
            try
            {
                var usuarios = _service.ObtenerUsuarios(pageNumber, pageSize);
                return Ok(new { isSuccess = true, data = usuarios });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerUsuarioPorId([FromRoute] int id)
        {
            try
            {
                UsuarioResponseDTO usuarioResponse = _service.ObtenerUsuarioDtoPorId(id);
                return Ok(new { isSuccess = true, data = usuarioResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario([FromRoute] int id)
        {
            try
            {
                _service.EliminarUsuario(id);
                return Ok(new { isSuccess = true, message = "Usuario eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message });
            }
        }
    }
}
