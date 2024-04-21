using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class CatProducto
{
    public int IdCatProducto { get; set; }

    [DisplayName("Descripcion de Producto")]
    public string? DescatProducto { get; set; }

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
    public virtual ICollection<Abastecimiento> Abastecimientos { get; set; } = new List<Abastecimiento>();
}
