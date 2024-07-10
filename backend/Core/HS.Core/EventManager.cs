using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HS
{
    public static class EventManager
    {
        private const string KeyContext = "GESTOR-EVENTOS";
        private static AsyncLocal<IEventSender> _sender = new AsyncLocal<IEventSender>();
        private static ResolveEventSender _resolve;

        public static IEventSender Sender
        {
            get
            {
                if (_sender.Value == null)
                    _sender.Value = _resolve?.Invoke();
                return _sender.Value;
            }
        }

        public static void SetEventSender(ResolveEventSender resolve)
        {
            _resolve = resolve;
        }

        [Obsolete]
        public static async Task LanzarEventoAsync<T>(T evento) where T : IDomainEvent
        {
            if (Sender != null)
                await Sender.SendAsync(evento);
        }
    }
}
