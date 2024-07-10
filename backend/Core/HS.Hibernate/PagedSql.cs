using HS.Pagination;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class PagedSql<T> : SqlQuery<PagedList<T>>, IPagedQuery<T>
        where T : class
    {
        public PagedSql(ISessionFactory sessionFactory)
            : base(sessionFactory) { }

        public int Pagina { get; set; }
        public int Numero { get; set; }
        public string Filtro { get; set; }

        protected virtual string GetCountSql()
        {
            return string.Format("Select Count(e.Id) From {0} as e Where e.Eliminado is false", typeof(T).Name);
        }

        protected override string GetSql()
        {
            return string.Format("Select * From {0} as e Where e.Eliminado is false", typeof(T).Name);
        }

        protected override PagedList<T> InnerExecute(IQuery query)
        {
            var qryCount = Session.CreateSQLQuery(GetCountSql());
            SetParameter(qryCount);
            var total = qryCount.FutureValue<int>();
            var lista = query.SetFirstResult((Pagina - 1) * Numero).SetMaxResults(Numero).Future<T>();
            return new PagedList<T>
            {
                Total = total.Value,
                Items = lista
            };
        }

        protected override async Task<PagedList<T>> InnerExecuteAsync(IQuery query)
        {
            var qryCount = Session.CreateSQLQuery(GetCountSql());
            SetParameter(qryCount);
            var total = await qryCount.UniqueResultAsync<int>();
            var lista = await query.SetFirstResult((Pagina - 1) * Numero).SetMaxResults(Numero).ListAsync<T>();
            return new PagedList<T>
            {
                Total = total,
                Items = lista
            };
        }
    }
}
