using System.ComponentModel;

namespace SistemaLavanderia.Models
{
    public class Abastecimiento
    {
        public int IdAbastecimiento { get; set; }
        public int? Categoria { get; set; }

        [DisplayName("Cantidad a Solicitar")]
        public int cantidadaIngresar { get; set; }

        [DisplayName("Producto")]
        public virtual CatProducto? CategoriaNavigation { get; set; }

        
    }
}
