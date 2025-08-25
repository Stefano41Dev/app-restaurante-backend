using app_restaurante_backend.Reports.DTOs;
using app_restaurante_backend.Reports.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_restaurante_backend.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {

        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("boleta/{id}")]
        public IActionResult GenerarBoleta(long id, [FromBody] InformacionAdicionalBoletaRequest info)
        {
            try
            {
                byte[] reporte = _reportService.GenerarBoleta(id, info);
                return File(reporte, "application/pdf", $"boleta_{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al generar la boleta.", error = ex.Message });
            }
        }

    }
}
