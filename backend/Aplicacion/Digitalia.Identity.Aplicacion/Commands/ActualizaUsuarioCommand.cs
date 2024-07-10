using Digitalia.Identity.Model;
using HS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Commands
{
    public class ActualizaUsuarioCommand: Command
    {
        public required ActualizaUsuarioRequest Data { get; set; }
    }
}
