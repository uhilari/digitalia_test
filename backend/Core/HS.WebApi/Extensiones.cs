using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HS
{
    public static class Extensiones
    {
        public static Guid GetIdUsuario(this ClaimsPrincipal principal)
        {
            var sid = principal.FindFirst(ClaimTypes.Sid);
            return sid.Value.Guid();
        }

        public static Guid GetIdPerfil(this ClaimsPrincipal principal)
        {
            var xperf = principal.FindFirst("x-perfil");
            return xperf.Value.Guid();
        }

        public static string GetRol(this ClaimsPrincipal principal)
        {
            var rol = principal.FindFirst(ClaimTypes.Role);
            return rol?.Value;
        }
    }
}
