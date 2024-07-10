using HS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Dominio.Errores
{
    public class IdentityErrorException(IEnumerable<IdentityError> errors) : BaseException(40000, "Identity no valido")
    {
        private readonly IEnumerable<IdentityError> _errors = errors;

        public override IEnumerable<Error> GetErrors()
        {
            return _errors.Select(e => new Error(40000, $"{e.Code} - {e.Description}"));
        }
    }
}
