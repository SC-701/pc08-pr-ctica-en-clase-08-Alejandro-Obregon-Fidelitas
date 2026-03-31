using Abstracciones.Interfaces.Servicios;

namespace Abstracciones.Modelos.Servicios
{
    public interface IProductosReglas
    {
        Task<decimal> CalcularPrecioUSD(decimal Precio);
    }
}
