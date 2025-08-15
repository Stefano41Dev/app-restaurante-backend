using System;
using System.Collections.Generic;

namespace app_restaurante_backend.Models;

public partial class CategoriasItem
{
    public short Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public double? PrecioMinimo { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<ItemsMenu> ItemsMenus { get; set; } = new List<ItemsMenu>();
}
