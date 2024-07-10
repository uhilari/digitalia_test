using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IEventSubscriber: IDisposable
    {
        void Subscribe<T>() where T : IDomainEvent;
    }
}
