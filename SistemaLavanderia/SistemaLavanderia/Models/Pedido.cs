using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class Pedido
{
    public int? Cliente { get; set; }

    public int? Servicio { get; set; }

    public int? Lavadora { get; set; }

    public int? CantPrendas { get; set; }

    public decimal? PrecioTotal { get; set; }

    public DateTime? FechaPedido { get; set; }

    public int? EstadoPedido { get; set; }

    public int IdPedido { get; set; }

    public virtual Cliente? ClienteNavigation { get; set; }

    public virtual Estado? EstadoPedidoNavigation { get; set; }

    public virtual Lavadora? LavadoraNavigation { get; set; }

    public virtual Servicio? ServicioNavigation { get; set; }
}
