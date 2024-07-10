using HS.Pagination;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Transform;

namespace HS
{
    public class PagedQuery<TEntity> : HqlQuery<PagedList<TEntity>>, IPagedQuery<TEntity>
      where TEntity : class
    {
        public PagedQuery(ISessionFactory sessionFactory) : base(sessionFactory) { }

        public int Pagina { get; set; }
        public int Numero { get; set; }
        public string Filtro { get; set; }

        protected virtual string GetCountHql()
        {
            return string.Format("Select Count(e.Id) From {0} as e Where e.Eliminado is false", typeof(TEntity).Name);
        }

        protected override string GetHql()
        {
            return string.Format("From {0} as e Where e.Eliminado is false", typeof(TEntity).Name);
        }

        protected override PagedList<TEntity> InnerExecute(IQuery query)
        {
            var qryCount = Session.CreateQuery(GetCountHql());
            SetParameter(qryCount);
            var total = qryCount.FutureValue<long>();
            var lista = query.SetFirstResult((Pagina - 1) * Numero).SetMaxResults(Numero).Future<TEntity>();
            return new PagedList<TEntity>
            {
                Total = total.Value,
                Items = lista
            };
        }

        protected override async Task<PagedList<TEntity>> InnerExecuteAsync(IQuery query)
        {
            var qryCount = Session.CreateQuery(GetCountHql());
            SetParameter(qryCount);

            var total = await qryCount.UniqueResultAsync<long>();

            var transformer = GetTransformer();
            if (transformer != null)
                query.SetResultTransformer(transformer);
            var lista = await query.SetFirstResult((Pagina - 1) * Numero).SetMaxResults(Numero).ListAsync<TEntity>();
            return new PagedList<TEntity>
            {
                Total = total,
                Items = lista
            };
        }
    }
}
