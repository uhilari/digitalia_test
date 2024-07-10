using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class CommandOption<TCommand>
        where TCommand: Command
    {
        public CommandOption(IServiceCollection container)
        {
            Container = container;
        }

        private IServiceCollection Container { get; }

        public void Unique()
        {
        }
    }
}
