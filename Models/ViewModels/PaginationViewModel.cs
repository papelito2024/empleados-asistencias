using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test2.Models.ViewModels
{
    public class PaginacionViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }  // Elementos de la página actual
        public int TotalItems { get; set; }  // Total de elementos (sin paginar)
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);  // Total de páginas
        public int PageSize { get; set; }  // Número de elementos por página
        public int CurrentPage { get; set; }  // Página actual
    }
}