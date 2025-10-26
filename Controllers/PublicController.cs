using app_restaurante_backend.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace app_restaurante_backend.Controllers
{
    [Route("api/public")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public PublicController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("LA API ES PUBLICA");
        }

        [HttpGet("send-test-event")]
        public async Task<IActionResult> SendTestEvent()
        {
            // Esto envía un mensaje a *TODOS* los clientes conectados
            await _hubContext.Clients.All.SendAsync(
                "ReceiveNotification", // Nombre del método que el cliente espera
                "Test Event",          // Parámetro 1: Título
                "Mensaje de prueba desde el backend sin usar el frontend." // Parámetro 2: Mensaje
            );

            return Ok("Mensaje de prueba de SignalR enviado.");
        }
    }
}
