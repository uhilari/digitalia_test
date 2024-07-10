using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class SessionDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : Command
    {
        private ICommandHandler<TCommand> _commandHandler;
        private ISessionFactory _sessionFactory;

        public SessionDecorator(ICommandHandler<TCommand> commandHandler, ISessionFactory sessionFactory)
        {
            _commandHandler = commandHandler;
            _sessionFactory = sessionFactory;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                CurrentSessionContext.Bind(session);
                await _commandHandler.ExecuteAsync(command);
                CurrentSessionContext.Unbind(_sessionFactory);
            }
        }
    }

    public class SessionDecorator<TCommand, TResultado> : ICommandHandler<TCommand, TResultado>
      where TCommand : Command<TResultado>
    {
        private ICommandHandler<TCommand, TResultado> _commandHandler;
        private ISessionFactory _sessionFactory;

        public SessionDecorator(ICommandHandler<TCommand, TResultado> commandHandler, ISessionFactory sessionFactory)
        {
            _commandHandler = commandHandler;
            _sessionFactory = sessionFactory;
        }

        public async Task<TResultado> ExecuteAsync(TCommand command)
        {
            TResultado resultado = default(TResultado);
            using (var session = _sessionFactory.OpenSession())
            {
                CurrentSessionContext.Bind(session);
                resultado = await _commandHandler.ExecuteAsync(command);
                CurrentSessionContext.Unbind(_sessionFactory);
            }
            return resultado;
        }
    }
}
