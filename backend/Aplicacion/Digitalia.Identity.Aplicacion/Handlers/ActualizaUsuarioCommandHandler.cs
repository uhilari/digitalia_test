using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Dominio;
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
    public class ActualizaUsuarioCommandHandler(IHttpContextAccessor httpContext, UserManager<Usuario> userManager) : ICommandHandler<ActualizaUsuarioCommand>
    {
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly UserManager<Usuario> _userManager = userManager;

        public async Task ExecuteAsync(ActualizaUsuarioCommand command)
        {
            var usuario = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name) ?? throw new NotFoundEntityException("Usuario no encontrado");
            usuario.Nombres = string.IsNullOrEmpty(command.Data.Nombres) ? usuario.Nombres : command.Data.Nombres;
            usuario.Apellidos = string.IsNullOrEmpty(command.Data.Apellidos) ? usuario.Apellidos : command.Data.Apellidos;
            usuario.Email = string.IsNullOrEmpty(command.Data.Email) ? usuario.Email : command.Data.Email;

            await _userManager.UpdateAsync(usuario);
        }
    }
}
