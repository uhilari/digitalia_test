using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IDataWriter
    {
        void Add<TEntity>(TEntity entidad) where TEntity : Entity;
        Task AddAsync<TEntity>(TEntity entidad) where TEntity : Entity;
    }
}
