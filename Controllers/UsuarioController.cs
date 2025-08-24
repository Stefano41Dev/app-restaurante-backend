using app_restaurante_backend.Custom;
using app_restaurante_backend.Models.DTOs.Usuario;
using app_restaurante_backend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
namespace app_restaurante_backend.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioService _service;
        private readonly IHubContext<NotificationHub> _hubContext;

        public UsuarioController(IUsuarioService service, IHubContext<NotificationHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarUsuario([FromBody] UsuarioRequestDTO usuarioDTORequest)
        {
           
            UsuarioResponseDTO usuarioResponse = _service.GuardarUsuario(usuarioDTORequest);
            await _hubContext.Clients.All.SendAsync("ActualizarUsuario", usuarioResponse);
            return Ok( usuarioResponse);
           
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
        public async Task<IActionResult> ActualizarUsuarioAsync([FromRoute]int id, [FromBody] UsuarioRequestDTO usuarioDTORequest)
        {
   
            UsuarioResponseDTO usuarioResponse = _service.ActualizarUsuario(id, usuarioDTORequest);
            await _hubContext.Clients.All.SendAsync("ActualizarUsuario", usuarioResponse);
            return Ok( usuarioResponse);

        }

        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
                var usuarios = _service.ObtenerUsuarios();
                return Ok( usuarios );   
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerUsuarioPorId([FromRoute] int id)
        {         
                UsuarioResponseDTO usuarioResponse = _service.ObtenerUsuarioDtoPorId(id);
                return Ok(usuarioResponse);          
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuarioAsync([FromRoute] int id)
        {
            
            _service.EliminarUsuario(id);
            await _hubContext.Clients.All.SendAsync("UsuarioEliminado", id);
            return Ok( "Usuario eliminado correctamente");
           
        }
    }
}
