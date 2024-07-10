using Microsoft.EntityFrameworkCore;
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
        public SessionDecorator(ICommandHandler<TCommand> commandHandler, IContextFactory contextFactory) {
            _commandHandler = commandHandler;
            _contextFactory = contextFactory;
        }

        private readonly ICommandHandler<TCommand> _commandHandler;
        private readonly IContextFactory _contextFactory;

        public async Task ExecuteAsync(TCommand command)
        {
            using(var context = _contextFactory.CreateDbContext())
            {
                await this._commandHandler.ExecuteAsync(command);
                
            }
        }
    }

    public class SessionDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : Command<TResult>
    {
        public SessionDecorator(ICommandHandler<TCommand, TResult> commandHandler, IContextFactory contextFactory)
        {
            _commandHandler = commandHandler;
            _contextFactory = contextFactory;
        }

        private readonly ICommandHandler<TCommand, TResult> _commandHandler;
        private readonly IContextFactory _contextFactory;

        public async Task<TResult> ExecuteAsync(TCommand command)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await _commandHandler.ExecuteAsync(command);
            }
        }
    }
}

