using HS.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class DataWriter : BaseEntityFramework, IDataWriter
    {
        public DataWriter(IContextFactory contextFactory): base(contextFactory) { }

        public void Add<TEntity>(TEntity entidad) where TEntity : Entity
        {
            DbContext.Set<TEntity>().Add(entidad);
        }

        public async Task AddAsync<TEntity>(TEntity entidad) where TEntity : Entity
        {
            await DbContext.Set<TEntity>().AddAsync(entidad);
        }
    }
}
