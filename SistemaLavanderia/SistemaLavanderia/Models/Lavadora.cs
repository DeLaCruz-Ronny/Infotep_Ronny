using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class Lavadora
{
    public int IdLavadoras { get; set; }

    public string? DesLavadoras { get; set; }

    public bool? Disponible { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
