using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class TransactionDecorator<TCommand> : BaseHibernate, ICommandHandler<TCommand>
      where TCommand : Command
    {
        private readonly ICommandHandler<TCommand> _handler;

        public TransactionDecorator(ISessionFactory sessionFactory, ICommandHandler<TCommand> handler)
            : base(sessionFactory)
        {
            _handler = handler;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            using (var tran = Session.BeginTransaction())
            {
                await _handler.ExecuteAsync(command);
                await tran.CommitAsync();
            }
        }
    }

    public class TransactionDecorator<TCommand, TResultado> : BaseHibernate, ICommandHandler<TCommand, TResultado>
      where TCommand : Command<TResultado>
    {
        private readonly ICommandHandler<TCommand, TResultado> _handler;

        public TransactionDecorator(ISessionFactory sessionFactory, ICommandHandler<TCommand, TResultado> handler)
            :base(sessionFactory)
        {
            _handler = handler;
        }

        public async Task<TResultado> ExecuteAsync(TCommand command)
        {
            var res = default(TResultado);

            using (var tran = Session.BeginTransaction())
            {
                res = await _handler.ExecuteAsync(command);
                await tran.CommitAsync();
            }

            return res;
        }
    }
}
