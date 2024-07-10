using Digitalia.Identity.Model;
using HS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Commands
{
    public class RegistroCommand: Command
    {
        public required RegistroRequest Data { get; set; }
    }
}
