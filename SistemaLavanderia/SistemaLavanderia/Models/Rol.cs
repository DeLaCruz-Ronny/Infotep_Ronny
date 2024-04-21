using System;
using System.Collections.Generic;

namespace SistemaLavanderia.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string? DesRol { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
