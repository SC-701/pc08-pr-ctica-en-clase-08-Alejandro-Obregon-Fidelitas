using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Servicios;

namespace Flujo
{
    public class SubcategoriaFlujo : ISubcategoriaFlujo
    {
        private ISubcategoriaDA _subcategoriaDA;

        public SubcategoriaFlujo(ISubcategoriaDA subcategoriaDA)
        {
            _subcategoriaDA = subcategoriaDA;
        }

        public Task<IEnumerable<Subcategoria>> Obtener(Guid Id)
        {
            return _subcategoriaDA.Obtener(Id);
        }
    }
}
