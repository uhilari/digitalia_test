using HS.Pagination;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class PagedCriteria<TEntity> : BaseHibernate, IPagedCriteria<TEntity>
        where TEntity : Entity
    {
        public PagedCriteria(ISessionFactory sessionFactory)
            : base(sessionFactory) { }

        public int Inicio { get; set; }
        public int Numero { get; set; }
        public Expression<Func<TEntity, bool>> Filtro { get; set; }

        public PagedList<TEntity> Execute()
        {
            var crtCount = Session.CreateCriteria<TEntity>()
                .Add(Restrictions.Where(Filtro))
                .Add(Restrictions.Eq("Eliminado", false))
                .SetProjection(Projections.RowCount());
            var criteria = Session.CreateCriteria<TEntity>()
                .Add(Restrictions.Where(Filtro))
                .Add(Restrictions.Eq("Eliminado", false));
            var total = crtCount.FutureValue<int>();
            var lista = criteria.SetFirstResult(Inicio).SetMaxResults(Numero).Future<TEntity>();
            return new PagedList<TEntity>
            {
                Total = total.Value,
                Items = lista
            };
        }

        public async Task<PagedList<TEntity>> ExecuteAsync()
        {
            var crtCount = Session.CreateCriteria<TEntity>()
                .Add(Restrictions.Where(Filtro))
                .Add(Restrictions.Eq("Eliminado", false))
                .SetProjection(Projections.RowCount());
            var criteria = Session.CreateCriteria<TEntity>()
                .Add(Restrictions.Where(Filtro))
                .Add(Restrictions.Eq("Eliminado", false));
            var totalTask = crtCount.UniqueResultAsync<int>();
            var listaTask = criteria.SetFirstResult(Inicio).SetMaxResults(Numero).ListAsync<TEntity>();
            await Task.WhenAll(totalTask, listaTask);
            return new PagedList<TEntity>
            {
                Total = totalTask.Result,
                Items = listaTask.Result
            };
        }
    }
}
