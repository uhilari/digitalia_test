using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class ContextFactory<TContext> : IContextFactory 
        where TContext: DbContext
    {
        public ContextFactory(IDbContextFactory<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly IDbContextFactory<TContext> _contextFactory;
        private DbContext? _dbContext = null;

        public DbContext CreateDbContext()
        {
            _dbContext = _contextFactory.CreateDbContext();
            return _dbContext;
        }

        public DbContext GetDbContext()
        {
            if (_dbContext == null)
            {
                throw new InvalidOperationException("No se a inicializado el DbContext");
            }
            return _dbContext;
        }
    }
}
