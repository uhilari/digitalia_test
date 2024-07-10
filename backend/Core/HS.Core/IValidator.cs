using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
  public interface IValidator<TCommand> where TCommand: Command
  {
    IAsyncEnumerable<Error> ValidarAsync(TCommand command);
  }

  public interface IValidator<TCommand, TResultado> where TCommand : Command<TResultado>
  {
    IAsyncEnumerable<Error> ValidarAsync(TCommand command);
  }
}
