using app_restaurante_backend.Models.Enums.Ordenes;
using System;
using System.Collections.Generic;

namespace app_restaurante_backend.Models.Entidades;

public partial class Mesa
{
    public short Id { get; set; }

    public string? Numero { get; set; }

    public short? Capacidad { get; set; }

    public EstadoMesa Estado { get; set; } = EstadoMesa.LIBRE;

    public bool? Activo { get; set; } = true;

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

}
