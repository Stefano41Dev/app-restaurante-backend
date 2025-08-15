using System;
using System.Collections.Generic;

namespace app_restaurante_backend.Models;

public partial class Ordene
{
    public long Id { get; set; }

    public string? CodigoOrden { get; set; }

    public short MesaId { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public TimeSpan? HoraCreacion { get; set; }

    public double? MontoSubtotal { get; set; }

    public double? MontoTotal { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<DetalleOrdene> DetalleOrdenes { get; set; } = new List<DetalleOrdene>();

    public virtual Mesa Mesa { get; set; } = null!;
}
