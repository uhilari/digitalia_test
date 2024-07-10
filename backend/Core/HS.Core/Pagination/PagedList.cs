using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Pagination
{
    public class PagedList<TDto>
      where TDto : class
    {
        public int Total { get; set; }
        public IEnumerable<TDto> Items { get; set; }

        public static PagedList<TDto> Vacio { get => new PagedList<TDto> { Total = 0 }; }
    }
}
