using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class Inventario
{
    public int IdProductos { get; set; }

    public string? DesProductos { get; set; }

    public int? Categoria { get; set; }

    public int? CantidadProducto { get; set; }

    public int? Servicio { get; set; }

    public virtual CatProducto? CategoriaNavigation { get; set; }

    public virtual Servicio? ServicioNavigation { get; set; }
}
