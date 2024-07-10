using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Dominio;
using Digitalia.Identity.Dominio.Errores;
using HS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Handlers
{
    public class CambioPwdCommandHandler(IHttpContextAccessor httpContext, UserManager<Usuario> userManager) : ICommandHandler<CambioPwdCommand>
    {
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly UserManager<Usuario> _userManager = userManager;

        public async Task ExecuteAsync(CambioPwdCommand command)
        {
            var usuario = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name) ?? throw new NotFoundEntityException("Usuario no encontrado");
            var pwdResult = await _userManager.ChangePasswordAsync(usuario, command.Data.AntiguoPassword, command.Data.NuevoPassword);
            if (!pwdResult.Succeeded)
            {
                throw new IdentityErrorException(pwdResult.Errors);
            }
        }
    }
}
