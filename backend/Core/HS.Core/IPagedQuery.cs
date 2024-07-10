using HS.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IPagedQuery<TEntity> : IQuery<PagedList<TEntity>>
      where TEntity : class
    {
        int Pagina { get; set; }
        int Numero { get; set; }
    }
}
