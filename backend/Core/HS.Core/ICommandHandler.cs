using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface ICommandHandler<TCommand>
      where TCommand : Command
    {
        Task ExecuteAsync(TCommand command);
    }

    public interface ICommandHandler<TCommand, TResultado>
      where TCommand : Command<TResultado>
    {
        Task<TResultado> ExecuteAsync(TCommand command);
    }
}
