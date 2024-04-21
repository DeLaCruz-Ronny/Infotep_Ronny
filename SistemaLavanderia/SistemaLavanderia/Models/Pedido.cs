using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class Pedido
{
    public int? Cliente { get; set; }

    public int? Servicio { get; set; }

    public int? Lavadora { get; set; }

    [DisplayName("Cantidad de Prendas")]
    public int? CantPrendas { get; set; }

    [DisplayName("Precio Total")]
    public decimal? PrecioTotal { get; set; }

    [DisplayName("Fecha Pedido")]
    public DateTime? FechaPedido { get; set; }

    [DisplayName("Estado del Pedido")]
    public int? EstadoPedido { get; set; }

    public int IdPedido { get; set; }

    [DisplayName("Cliente")]
    public virtual Cliente? ClienteNavigation { get; set; }

    [DisplayName("Estado")]
    public virtual Estado? EstadoPedidoNavigation { get; set; }

    [DisplayName("Lavadora")]
    public virtual Lavadora? LavadoraNavigation { get; set; }

    [DisplayName("Servicio")]
    public virtual Servicio? ServicioNavigation { get; set; }
}
