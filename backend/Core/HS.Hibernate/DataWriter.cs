using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class DataWriter : BaseHibernate, IDataWriter
    {
        public DataWriter(ISessionFactory sessionFactory) : base(sessionFactory) { }

        public void Add<TEntity>(TEntity entidad) where TEntity : Entity
        {
            Session.Save(entidad);
        }

        public async Task AddAsync<TEntity>(TEntity entidad) where TEntity : Entity
        {
            await Session.SaveAsync(entidad);
        }
    }
}
