using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Model
{
    public class CambioPwdRequest
    {
        public required string AntiguoPassword { get; set; }
        public required string NuevoPassword { get; set; }
    }
}
