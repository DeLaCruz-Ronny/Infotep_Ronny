using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Usuario1 { get; set; }

    public string? Clave { get; set; }

    public int? Rol { get; set; }

    public virtual Rol? RolNavigation { get; set; }
}
