using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Dominio;
using Digitalia.Identity.Model;
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
    public class UsuarioActualCommandHandler(IHttpContextAccessor httpContext, UserManager<Usuario> userManager) : ICommandHandler<UsuarioActualCommand, UsuarioActualResponse>
    {
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly UserManager<Usuario> _userManager = userManager;

        public async Task<UsuarioActualResponse> ExecuteAsync(UsuarioActualCommand command)
        {
            var usuario = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name) ?? throw new NotFoundEntityException("Usuario no encontrado");

            return new UsuarioActualResponse
            {
                Username = usuario.UserName,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email
            };
        }
    }
}
