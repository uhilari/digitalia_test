using Digitalia.Identity.Model;
using HS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Commands
{
    public class CambioPwdCommand : Command
    {
        public required CambioPwdRequest Data { get; set; }
    }
}
