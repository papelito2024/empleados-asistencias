using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;



public class DataService
{
    private readonly DbContext _context;

    public DataService(DbContext context)
    {
        _context = context;
    }

    // Método genérico para obtener datos con filtros, joins y paginación
    public PaginatedResult<TResult> GetPaginatedDataWithJoin<T, TJoin, TResult>(
        Expression<Func<T, TJoin, bool>> joinCondition, // Condición para el join
        Dictionary<string, object> filters = null,
        int pageIndex = 1,
        int pageSize = 10,
        Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null)
        where T : class
        where TJoin : class
        where TResult : class
    {
        var query = _context.Set<T>().AsQueryable();

        // Aplicar filtros si se proporcionan
        if (filters != null && filters.Any())
        {
            foreach (var filter in filters)
            {
                query = ApplyFilter(query, filter.Key, filter.Value);
            }
        }

        // Convertir a tipo resultado
        var resultQuery = query.Select(x => (TResult)Activator.CreateInstance(typeof(TResult), x));

        // Aplicar ordenamiento si se proporciona
        if (orderBy != null)
        {
            resultQuery = orderBy(resultQuery);
        }

        // Obtener el total de elementos
        var totalCount = resultQuery.Count();

        // Realizar la paginación
        var items = resultQuery.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        // Crear y retornar el resultado paginado
        return new PaginatedResult<TResult>
        {
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = items
        };
    }

    // Método para aplicar un filtro dinámico
    private IQueryable<T> ApplyFilter<T>(IQueryable<T> query, string propertyName, object value) where T : class
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var equality = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

        return query.Where(lambda);
    }
}


public class PaginatedResult<T>
{
    public int TotalCount { get; set; }  // Número total de elementos
    public int PageIndex { get; set; }   // Índice de la página actual
    public int PageSize { get; set; }    // Número de elementos por página
    public List<T> Items { get; set; }   // Lista de elementos (por ejemplo, departamentos, empleados, etc.)

    public PaginatedResult()
    {
        Items = new List<T>();
    }
}
