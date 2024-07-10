using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Dominio;
using HS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Validators
{
    public class RegistroCommandValidator(UserManager<Usuario> userManager) : IValidator<RegistroCommand>
    {
        private readonly UserManager<Usuario> _userManager = userManager;

        public async IAsyncEnumerable<Error> ValidarAsync(RegistroCommand command)
        {
            var usuario = await _userManager.FindByNameAsync(command.Data.Username);
            if (usuario != null)
            {
                yield return new Error(40001, "Ya existe el nombre de usuario");
            }

            usuario = await _userManager.FindByEmailAsync(command.Data.Email);
            if (usuario != null)
            {
                yield return new Error(40002, "Ya existe el email de usuario");
            }
        }
    }
}
