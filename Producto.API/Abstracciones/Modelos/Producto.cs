using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "La propiedad nombre es requerida")]
        [StringLength(120, ErrorMessage = "La propiedad nombre debe ser mayor a 4 caracteres y menor a 120", MinimumLength = 4)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La propiedad descripcion es requerida")]
        [StringLength(4000, ErrorMessage = "La propiedad descripcion debe ser mayor a 5 caracteres y menor a 4000", MinimumLength = 5)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La propiedad precio es requerida")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "La propiedad stock es requerida")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "La propiedad codigo de barras es requerida")]
        public string CodigoBarras { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        public Guid Id { get; set; }
        public string SubCategoria { get; set; }
        public string Categoria { get; set; }
    }

    public class ProductoCalculado: ProductoResponse
    {
        public decimal PrecioUSD { get; set; }
    }
}
