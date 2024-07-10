using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IEventHandler<T> where T : IDomainEvent
    {
        Task EjecutarAsync(T evento);
    }
}
