using Digitalia.Identity.Model;
using HS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Commands
{
    public class CreateTokenCommand: Command<CreateTokenResponse>
    {
        public required CreateTokenRequest Data { get; set; }
    }
}
