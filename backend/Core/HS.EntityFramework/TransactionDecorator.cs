using HS.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class TransactionDecorator<TCommand> : BaseEntityFramework, ICommandHandler<TCommand>
        where TCommand : Command
    {
        public TransactionDecorator(ICommandHandler<TCommand> handler, IContextFactory dbContextFactory)
            : base(dbContextFactory)
        {
            _commandHandler = handler;
        }

        private readonly ICommandHandler<TCommand> _commandHandler;

        public async Task ExecuteAsync(TCommand command)
        {
            using (var tran = DbContext.Database.BeginTransaction())
            {
                await _commandHandler.ExecuteAsync(command);
                DbContext.SaveChanges();
                tran.Commit();
            }
        }
    }

    public class TransactionDecorator<TCommand, TResult> : BaseEntityFramework, ICommandHandler<TCommand, TResult>
        where TCommand : Command<TResult>
    {
        public TransactionDecorator(ICommandHandler<TCommand, TResult> handler, IContextFactory dbContextFactory)
            : base(dbContextFactory)
        {
            _commandHandler = handler;
        }

        private readonly ICommandHandler<TCommand, TResult> _commandHandler;

        public async Task<TResult> ExecuteAsync(TCommand command)
        {
            using (var tran = DbContext.Database.BeginTransaction())
            {
                var result = await _commandHandler.ExecuteAsync(command);
                DbContext.SaveChanges();
                tran.Commit();
                return result;
            }
        }
    }
}
