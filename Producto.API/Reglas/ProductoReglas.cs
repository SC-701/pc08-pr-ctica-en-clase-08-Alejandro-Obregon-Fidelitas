using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Servicios;

namespace Reglas
{
    public class ProductoReglas : IProductosReglas
    {
        private readonly ITipoCambioServicio _tipoCambioServicio;
        private readonly IConfiguracion _configuracion;

        public ProductoReglas(ITipoCambioServicio tipoCambioServicio, IConfiguracion configuracion)
        {
            _tipoCambioServicio = tipoCambioServicio;
            _configuracion = configuracion;
        }

        public async Task<decimal> CalcularPrecioUSD(decimal Precio)
        {
            var resultadoTipoDeCambio = await _tipoCambioServicio.ObtenerTipoCambio();
            var datoTipoCambio = resultadoTipoDeCambio.datos.FirstOrDefault();
            var indicadorTipoCambio = datoTipoCambio.indicadores.FirstOrDefault();
            var serieTipoCambio = indicadorTipoCambio.series.FirstOrDefault();

            decimal tipoCambio = Convert.ToDecimal(serieTipoCambio.valorDatoPorPeriodo);
            decimal precioEnUSD = Precio / tipoCambio;

            return precioEnUSD;
        }
    }
}
