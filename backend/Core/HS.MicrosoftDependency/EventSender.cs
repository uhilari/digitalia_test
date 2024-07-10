using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Windsor
{
    public class EventSender : IEventSender
    {
        private IServiceProvider _serviceProvider;

        public EventSender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync<T>(T evento) where T : IDomainEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<T>>();
            var taskHandlers = new List<Task>();
            foreach (var handler in handlers)
            {
                taskHandlers.Add(handler.EjecutarAsync(evento));
            }
            await Task.WhenAll(taskHandlers.ToArray());
        }
    }
}
