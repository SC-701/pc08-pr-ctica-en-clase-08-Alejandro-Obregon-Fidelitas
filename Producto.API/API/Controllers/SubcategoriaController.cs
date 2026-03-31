using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoriaController : ControllerBase, ISubcategoriaController
    {
        private ISubcategoriaFlujo _subcategoriaFlujo;
        private ILogger<SubcategoriaController> _logger;

        public SubcategoriaController(ISubcategoriaFlujo subcategoriaFlujo, ILogger<SubcategoriaController> logger)
        {
            _subcategoriaFlujo = subcategoriaFlujo;
            _logger = logger;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener(Guid Id)
        {
            var resultado = await _subcategoriaFlujo.Obtener(Id);
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }
    }
}
