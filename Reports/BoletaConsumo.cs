using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Reports.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace app_restaurante_backend.Reports
{
    public class BoletaConsumo : IDocument
    {

        private readonly OrdenResponseDto _orden;
        private readonly InformacionAdicionalBoletaRequest _informacionAdicional;

        public BoletaConsumo(OrdenResponseDto orden, InformacionAdicionalBoletaRequest info)
        {
            _orden = orden;
            _informacionAdicional = info;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.ContinuousSize(80, Unit.Millimetre);
                    page.Margin(8);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    // ENCABEZADO
                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("BOLETA DE VENTA ELECTRÓNICA").Bold().FontSize(12);
                        col.Item().AlignCenter().Text("RUC: 12345678901").FontSize(10);
                        col.Spacing(10);
                        col.Item().PaddingBottom(8);

                        col.Item().AlignCenter().Row(row =>
                        {
                            // -- Logo --
                            row.RelativeItem(1).AlignCenter().Image("Assets/logo.png");

                            // Información de la orden
                            row.RelativeItem(1).AlignCenter().PaddingTop(25).Column(column =>
                            {
                                column.Item().AlignLeft().Text($"{_orden.CodigoOrden}").Bold().FontSize(10);
                                column.Item().AlignLeft().Text($"MESA: {_orden.NumeroMesa}").FontSize(10);
                                column.Item().AlignLeft().Text($"FECHA: {_orden.FechaCreacion:dd/MM/yyyy}").FontSize(10);
                                column.Item().AlignLeft().Text($"HORA: {_orden.HoraCreacion:hh\\:mm\\:ss}").FontSize(10);
                            });
                        });

                        col.Item().PaddingVertical(5).PaddingBottom(5).LineHorizontal(1);
                        col.Spacing(5);
                    });

                    // CONTENIDO CENTRAL
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // ITEM
                            columns.RelativeColumn(1); // CANT
                            columns.RelativeColumn(1.5f); // PRE UND
                            columns.RelativeColumn(1.5f); // TOTAL
                        });

                        // Encabezado de la tabla
                        table.Header(header =>
                        {
                            header.Cell().Text("ITEM").Bold();
                            header.Cell().AlignCenter().Text("CANT").Bold().FontSize(10);
                            header.Cell().AlignRight().Text("PRE. U.").Bold().FontSize(10);
                            header.Cell().AlignRight().Text("TOTAL").Bold().FontSize(10);
                        });

                        // Filas de detalles
                        foreach (var detalle in _orden.Detalles)
                        {
                            table.Cell().Text(detalle.NombreItem).FontSize(10);
                            table.Cell().AlignCenter().Text(detalle.Cantidad.ToString()).FontSize(10);
                            table.Cell().AlignRight().Text($"{detalle.PrecioUnitario:S/ #,##0.00}").FontSize(10);
                            table.Cell().AlignRight().Text($"{detalle.Total:S/ #,##0.00}").FontSize(10);
                        }
                    });

                    // PIE DE PÁGINA
                    page.Footer().Column(col =>
                    {
                        col.Item().PaddingTop(10).LineHorizontal(1);

                        // Cálculo de IGV total
                        double igvTotal = _orden.Detalles.Sum(d => d.Igv);
                        decimal montoTotalDecimal = (decimal)_orden.MontoTotal;
                        decimal cambio = _informacionAdicional.MontoPagado - montoTotalDecimal;

                        col.Item().AlignRight().Row(row =>
                        {
                            row.RelativeItem().Text("SUBTOTAL:").Bold().FontSize(10);
                            row.ConstantItem(80, Unit.Point).AlignRight().Text($"{_orden.MontoSubtotal:S/ #,##0.00}").FontSize(10);
                        });
                        col.Item().AlignRight().Row(row =>
                        {
                            row.RelativeItem().Text("IGV (18%):").Bold().FontSize(10);
                            row.ConstantItem(80, Unit.Point).AlignRight().Text($"{igvTotal:S/ #,##0.00}").FontSize(10);
                        });
                        col.Item().AlignRight().Row(row =>
                        {
                            row.RelativeItem().Text("IMPORTE TOTAL:").Bold().FontSize(10);
                            row.ConstantItem(80, Unit.Point).AlignRight().Text($"{_orden.MontoTotal:S/ #,##0.00}").FontSize(10);
                        });
                        col.Item().AlignRight().Row(row =>
                        {
                            row.RelativeItem().Text("PAGO:").Bold().FontSize(10);
                            row.ConstantItem(80, Unit.Point).AlignRight().Text($"{_informacionAdicional.MontoPagado:S/ #,##0.00}").FontSize(10);
                        });
                        col.Item().AlignRight().Row(row =>
                        {
                            row.RelativeItem().Text("CAMBIO:").Bold().FontSize(10);
                            row.ConstantItem(80, Unit.Point).AlignRight().Text($"{cambio:S/ #,##0.00}").FontSize(10);
                        });

                        col.Spacing(10);
                        col.Item().PaddingTop(8);
                        col.Item().AlignCenter().Text("¡GRACIAS POR SU CONSUMO!").Bold();
                        col.Item().AlignCenter().Text("¡VUELVA PRONTO!").Bold();
                    });
                });
        }
    }
}
