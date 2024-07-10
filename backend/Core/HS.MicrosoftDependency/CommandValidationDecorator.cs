using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class CommandValidationDecorator<TCommand> : ICommandHandler<TCommand>
      where TCommand : Command
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandHandler<TCommand> _handler;

        public CommandValidationDecorator(IServiceProvider serviceProvider, ICommandHandler<TCommand> handler)
        {
            _serviceProvider = serviceProvider;
            _handler = handler;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            var validators = _serviceProvider.GetServices<IValidator<TCommand>>();
            var errores = new List<Error>();
            foreach (var validator in validators)
            {
                errores.AddRange(await validator.ValidarAsync(command).ToListAsync());
            }
            errores = errores.Where(e => e != null).ToList();
            if (errores.Count > 0)
                throw new CommandNoValidException(errores);
            await _handler.ExecuteAsync(command);
        }
    }

    public class CommandValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
      where TCommand : Command<TResult>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandHandler<TCommand, TResult> _handler;

        public CommandValidationDecorator(IServiceProvider serviceProvider, ICommandHandler<TCommand, TResult> handler)
        {
            _serviceProvider = serviceProvider;
            _handler = handler;
        }

        public async Task<TResult> ExecuteAsync(TCommand command)
        {
            var validators = _serviceProvider.GetServices<IValidator<TCommand, TResult>>();
            var errores = new List<Error>();
            foreach (var validator in validators)
            {
                errores.AddRange(await validator.ValidarAsync(command).ToListAsync());
            }
            errores = errores.Where(e => e != null).ToList();
            if (errores.Count > 0)
                throw new CommandNoValidException(errores);
            return await _handler.ExecuteAsync(command);
        }
    }
}
