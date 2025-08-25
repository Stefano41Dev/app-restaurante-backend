using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Reports.DTOs;
using app_restaurante_backend.Service.Interfaces;
using QuestPDF.Fluent;

namespace app_restaurante_backend.Reports.Service
{
    public class ReportService
    {

        private readonly IOrdenService _ordenService;

        public ReportService(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        public byte[] GenerarBoleta(long idOrden, InformacionAdicionalBoletaRequest info)
        {
            OrdenResponseDto? orden = _ordenService.ObtenerOrden(idOrden);
            if (info.MontoPagado < (decimal)orden.MontoTotal)
            {
                throw new Exception("El monto pagado no puede ser menor al monto total de la orden.");
            }
            var documento = new BoletaConsumo(orden, info);
            return documento.GeneratePdf();
        }

    }
}
