using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    [DisplayName("Nombre de Usuario")]
    public string? Usuario1 { get; set; }

    public string? Clave { get; set; }

    public int? Rol { get; set; }

    public virtual Rol? RolNavigation { get; set; }
}
