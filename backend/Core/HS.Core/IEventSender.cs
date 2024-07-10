using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IEventSender
    {
        Task SendAsync<T>(T evento) where T : IDomainEvent;
    }
}
