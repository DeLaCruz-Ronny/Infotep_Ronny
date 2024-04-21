using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class CatProducto
{
    public int IdCatProducto { get; set; }

    public string? DescatProducto { get; set; }

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
