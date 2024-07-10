using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.EntityFramework
{
    public abstract class BaseEntityFramework
    {
        protected BaseEntityFramework(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly IContextFactory _contextFactory;

        private DbContext? _dbContext = null;

        protected DbContext DbContext { get => _dbContext ?? (_dbContext = _contextFactory.GetDbContext()); }
    }
}
