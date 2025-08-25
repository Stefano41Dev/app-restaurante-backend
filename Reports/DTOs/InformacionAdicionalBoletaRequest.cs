using System.Text.Json.Serialization;

namespace app_restaurante_backend.Reports.DTOs
{

    public record InformacionAdicionalBoletaRequest(
        [property:JsonPropertyName("monto_pagado")]
        decimal MontoPagado
    ) { }

}
