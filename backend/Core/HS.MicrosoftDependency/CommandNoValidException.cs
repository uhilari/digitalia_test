using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class CommandNoValidException : BaseException
    {
        private IEnumerable<Error> _errores;

        public CommandNoValidException(IEnumerable<Error> errores)
        {
            Codigo = 40010;
            _errores = errores;
        }

        public override IEnumerable<Error> GetErrors() => _errores;

        public override string Message => "El Command no es Válido";
    }
}
