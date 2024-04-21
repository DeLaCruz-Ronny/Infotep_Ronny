using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    [DisplayName("Descripcion de Servicio")]
    public string? DesServicio { get; set; }

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
