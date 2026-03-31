using System.Drawing;
using System.Numerics;
using System.Reflection;
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class SubcategoriaDA : ISubcategoriaDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public SubcategoriaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }
        public async Task<IEnumerable<Subcategoria>> Obtener(Guid Id)
        {
            string query = @"ObtenerSubcategorias";
            var resultadoConsulta = await _sqlConnection.QueryAsync<Subcategoria>(query,
                new { Id = Id });
            return resultadoConsulta;
        }
    }
}