using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        [BindProperty]
        public ProductoResponse productoResponse { get; set; }
        [BindProperty]
        public List<SelectListItem> categorias { get; set; }
        [BindProperty]
        public List<SelectListItem> subcategorias { get; set; }
        [BindProperty]
        public Guid categoriaseleccionada { get; set; }
        [BindProperty]
        public Guid subcategoriaseleccionada { get; set; }
        public async Task<ActionResult> OnGet(Guid? id)
        {
            if(id==Guid.Empty)
            {
                return NotFound();
            }
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if(respuesta.StatusCode==HttpStatusCode.OK)
            {
                await ObtenerCategorias();
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true };
                productoResponse = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones);
                if(productoResponse!=null)
                {
                    categoriaseleccionada = Guid.Parse(categorias.Where(m=>m.Text==productoResponse.Categoria).FirstOrDefault().Value);
                    subcategorias = (await ObtenerSubcategorias(categoriaseleccionada)).Select(m=>
                    new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nombre,
                        Selected = m.Nombre==productoResponse.SubCategoria
                    }).ToList();
                    subcategoriaseleccionada= Guid.Parse(subcategorias.Where(m => m.Text == productoResponse.SubCategoria).FirstOrDefault().Value);
                }
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            var cliente = new HttpClient();
            var respuesta = await cliente.PutAsJsonAsync<ProductoRequest>(string.Format(endpoint, productoResponse.Id), new ProductoRequest
            { 
                IdSubCategoria = subcategoriaseleccionada,
                Nombre = productoResponse.Nombre,
                Descripcion = productoResponse.Descripcion,
                Precio = productoResponse.Precio,
                Stock = productoResponse.Stock,
                CodigoBarras = productoResponse.CodigoBarras
            });
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
        }

        private async Task ObtenerCategorias()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true };
            var resultadodeserializado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);
            categorias = resultadodeserializado.Select(m=>
            new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre,
            }
            ).ToList();
        }

        private async Task<List<Subcategoria>> ObtenerSubcategorias(Guid categoriaId)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubcategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, categoriaId));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if(respuesta.StatusCode==HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Subcategoria>>(resultado, opciones);
            }
            return new List<Subcategoria>();
        }

        public async Task<JsonResult> OnGetObtenerSubcategorias(Guid categoriaID)
        {
            var subcategorias = await ObtenerSubcategorias(categoriaID);
            return new JsonResult(subcategorias);
        }
    }
}
