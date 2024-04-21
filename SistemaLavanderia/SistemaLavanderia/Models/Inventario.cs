using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class Inventario
{
    public int IdProductos { get; set; }

    [DisplayName("Descripcion de Producto")]
    public string? DesProductos { get; set; }

    public int? Categoria { get; set; }

    [DisplayName("Cantidad de Producto")]
    public int? CantidadProducto { get; set; }

    public int? Servicio { get; set; }

    [DisplayName("Categoria de Producto")]
    public virtual CatProducto? CategoriaNavigation { get; set; }

    [DisplayName("Servicio")]
    public virtual Servicio? ServicioNavigation { get; set; }
}
