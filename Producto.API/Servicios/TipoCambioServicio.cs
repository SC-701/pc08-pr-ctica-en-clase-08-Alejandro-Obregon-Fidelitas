using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using System.Text.Json;

namespace Servicios
{
    public class TipoCambioServicio : ITipoCambioServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClient;

        public TipoCambioServicio(IConfiguracion configuracion, IHttpClientFactory httpClient)
        {
            _configuracion = configuracion;
            _httpClient = httpClient;
        }

        public async Task<TipoCambio> ObtenerTipoCambio()
        {
            var endPoint = _configuracion.ObtenerMetodo("BancoCentralCR", "ObtenerCambioColonesADolares");
            var servicioRegistro = _httpClient.CreateClient();
            string idiomaDeLaPeticion = _configuracion.ObtenerValor("idiomaDeLaPeticion");
            var urlBase = _configuracion.ObtenerValor("BancoCentralCR:UrlBase");
            var token = _configuracion.ObtenerValor("BancoCentralCR:Token");

            var urlCompleta = string.Format(endPoint, DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), idiomaDeLaPeticion);

            var request = new HttpRequestMessage(HttpMethod.Get, urlCompleta);
            request.Headers.Add("Authorization", $"Bearer {token}");

            var respuesta = await servicioRegistro.SendAsync(request);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadoDeserializado = JsonSerializer.Deserialize<TipoCambio>(resultado, opciones);
            return resultadoDeserializado;
        }
    }
}
