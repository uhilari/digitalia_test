using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public interface IEntorno
    {
        Guid IdUsuario { get; }
        Guid IdPerfil { get; }
        string Rol { get; }
    }
}
