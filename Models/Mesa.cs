using System;
using System.Collections.Generic;

namespace app_restaurante_backend.Models;

public partial class Mesa
{
    public short Id { get; set; }

    public string? Numero { get; set; }

    public short? Capacidad { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();
}
