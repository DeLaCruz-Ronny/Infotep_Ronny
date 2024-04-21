using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class Lavadora
{
    public int IdLavadoras { get; set; }

    [DisplayName("Descripcion de Lavadora")]
    public string? DesLavadoras { get; set; }

    [DisplayName("Esta Disponible?")]
    public bool? Disponible { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
