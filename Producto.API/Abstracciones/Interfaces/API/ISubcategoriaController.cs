using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ISubcategoriaController
    {
        Task<IActionResult> Obtener(Guid Id);
    }
}
