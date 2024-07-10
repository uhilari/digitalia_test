using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Dominio;
using Digitalia.Identity.Dominio.Errores;
using HS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Handlers
{
    public class RegistroCommandHandler(UserManager<Usuario> userManager) : ICommandHandler<RegistroCommand>
    {
        private readonly UserManager<Usuario> _userManager = userManager;

        public async Task ExecuteAsync(RegistroCommand command)
        {
            var usuario = new Usuario { 
                UserName = command.Data.Username, 
                Nombres = command.Data.Nombres,
                Apellidos = command.Data.Apellidos,
                Email = command.Data.Email 
            };
            var createResult = await _userManager.CreateAsync(usuario, command.Data.Password);
            if (!createResult.Succeeded)
            {
                throw new IdentityErrorException(createResult.Errors);
            }
        }
    }
}
