using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Pagination
{
    public class PagedListCommand<TDto> : Command<PagedList<TDto>>
      where TDto : class
    {
        public int Pagina { get; set; } = 1;
        public int Limite { get; set; } = 15;
        public string Filtro { get; set; } = "";
    }
}
