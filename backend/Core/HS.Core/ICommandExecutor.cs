using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface ICommandExecutor
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : Command;
        Task<TResultado> ExecuteAsync<TResultado, TCommand>(TCommand command) where TCommand : Command<TResultado>;
    }
}
