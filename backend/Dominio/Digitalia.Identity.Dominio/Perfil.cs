using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Dominio
{
    public class Perfil: IdentityRole<Guid>
    {
        public Perfil()
        {
            Id = Guid.NewGuid();
        }
    }
}
