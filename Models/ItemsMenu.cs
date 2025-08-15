using System;
using System.Collections.Generic;

namespace app_restaurante_backend.Models;

public partial class ItemsMenu
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public double? Precio { get; set; }

    public string? EnlaceImagen { get; set; }

    public short CategoriaId { get; set; }

    public string? Estado { get; set; }

    public bool? Activo { get; set; }

    public virtual CategoriasItem Categoria { get; set; } = null!;

    public virtual ICollection<DetalleOrdene> DetalleOrdenes { get; set; } = new List<DetalleOrdene>();
}
