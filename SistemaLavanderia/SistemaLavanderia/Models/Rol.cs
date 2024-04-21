using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SistemaLavanderia.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    [DisplayName("Descripcion de Rol")]
    public string? DesRol { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
