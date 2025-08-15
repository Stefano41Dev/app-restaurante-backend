using System;
using System.Collections.Generic;

namespace app_restaurante_backend.Models.Entidades;

public partial class DetalleOrdene
{
    public long Id { get; set; }

    public long OrdenId { get; set; }

    public int PlatoId { get; set; }

    public int? Cantidad { get; set; }

    public double? PrecioUnitario { get; set; }

    public double? Igv { get; set; }

    public double? Subtotal { get; set; }

    public double? Total { get; set; }

    public bool? Activo { get; set; }

    public virtual Ordene Orden { get; set; } = null!;

    public virtual ItemsMenu Plato { get; set; } = null!;
}
