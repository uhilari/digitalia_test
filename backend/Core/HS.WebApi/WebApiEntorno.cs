using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class WebApiEntorno: IEntorno
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebApiEntorno(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid IdUsuario => _httpContextAccessor.HttpContext.User.GetIdUsuario();

        public string Rol => _httpContextAccessor.HttpContext.User.GetRol();
        public IServiceProvider Services => _httpContextAccessor.HttpContext.RequestServices;

        public Guid IdPerfil => _httpContextAccessor.HttpContext.User.GetIdPerfil();
    }
}
