using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace test2.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TablePaginator<T> where T : class
    {
        private readonly IQueryable<T> _queryableData;

        public TablePaginator(IQueryable<T> queryableData)
        {
            _queryableData = queryableData;
        }

        // Método para aplicar filtros de forma asincrónica
        public IQueryable<T> Filter(Func<T, bool> predicate)
        {
            return _queryableData.Where(predicate).AsQueryable();
        }

        // Método para aplicar paginación de forma asincrónica
        public async Task<IEnumerable<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        // Método combinado: aplica filtro y luego realiza paginación
        public async Task<IEnumerable<T>> FilterAndPaginateAsync(Func<T, bool> filterPredicate, int pageNumber, int pageSize)
        {
            var filteredData = _queryableData.Where(filterPredicate).AsQueryable();

            return await PaginateAsync(filteredData, pageNumber, pageSize);
        }
    }
}