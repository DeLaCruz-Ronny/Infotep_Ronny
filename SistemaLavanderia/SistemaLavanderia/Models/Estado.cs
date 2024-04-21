using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? DesEstado { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
