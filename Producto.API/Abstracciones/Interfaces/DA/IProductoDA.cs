using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IProductoDA
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoCalculado> Obtener(Guid Id);
        Task<Guid> Agregar(ProductoRequest producto);
        Task<Guid> Editar(Guid Id, ProductoRequest producto);
        Task<Guid> Eliminar(Guid Id);
    }
}
