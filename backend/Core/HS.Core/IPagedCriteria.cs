using HS.Pagination;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HS
{
    public  interface IPagedCriteria<TEntity> : IQuery<PagedList<TEntity>>
        where TEntity: Entity
    {
        int Inicio { get; set; }
        int Numero { get; set; }
        Expression<Func<TEntity, bool>> Filtro { get; set; }
    }
}
