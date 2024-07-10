using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Dominio
{
    public class Usuario: IdentityUser<Guid>
    {
        public Usuario()
        {
            Id = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }
        public string Nombres { get; set; } = String.Empty;
        public string Apellidos { get; set; } = String.Empty;
    }
}
