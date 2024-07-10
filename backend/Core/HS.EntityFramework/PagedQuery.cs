using HS.EntityFramework;
using HS.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class PagedQuery<TEntity> : BaseEntityFramework, IPagedQuery<TEntity>
        where TEntity : Entity
    {
        public PagedQuery(IContextFactory contextFactory) : base(contextFactory) { }

        public int Pagina { get; set; }
        public int Numero { get; set; }

        public PagedList<TEntity> Execute()
        {
            var setEntity = DbContext.Set<TEntity>();
            var count = setEntity.Count();
            var list = setEntity
                .OrderBy(c => c.Id)
                .Skip((Pagina - 1) * Numero)
                .Take(Numero)
                .ToList();

            return new PagedList<TEntity>
            {
                Items = list,
                Total = count
            };
        }

        public async Task<PagedList<TEntity>> ExecuteAsync()
        {
            var setEntity = DbContext.Set<TEntity>();
            var count = await setEntity.CountAsync();
            var list = await setEntity
                .OrderBy(c => c.Id)
                .Skip((Pagina - 1) * Numero)
                .Take(Numero)
                .ToListAsync();
            return new PagedList<TEntity>
            {
                Items = list,
                Total = count
            };
        }
    }
}
