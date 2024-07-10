using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class CommandExecutor: ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider.NoEsNull(nameof(serviceProvider));
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : Command
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
                await handler.ExecuteAsync(command);
            }
        }

        public async Task<TResultado> ExecuteAsync<TResultado, TCommand>(TCommand command) where TCommand : Command<TResultado>
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand, TResultado>>();
                return await handler.ExecuteAsync(command);
            }
        }
    }
}
