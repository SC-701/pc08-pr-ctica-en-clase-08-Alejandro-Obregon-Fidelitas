using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Servicios;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private IProductoDA _productoDA;
        private IProductosReglas _productoReglas;

        public ProductoFlujo(IProductoDA productoDA, IProductosReglas productoReglas)
        {
            _productoDA = productoDA;
            _productoReglas = productoReglas;
        }

        public Task<Guid> Agregar(ProductoRequest producto)
        {
            return _productoDA.Agregar(producto);
        }

        public Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            return _productoDA.Editar(Id, producto);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _productoDA.Eliminar(Id);
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return _productoDA.Obtener();
        }

        public async Task<ProductoCalculado> Obtener(Guid Id)
        {
            var producto = await _productoDA.Obtener(Id);
            producto.PrecioUSD = await _productoReglas.CalcularPrecioUSD(producto.Precio);
            return producto;
        }
    }
}
