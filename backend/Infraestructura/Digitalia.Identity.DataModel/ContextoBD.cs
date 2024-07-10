using Digitalia.Identity.Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.DataModel
{
    public class ContextoBD: IdentityDbContext<Usuario, Perfil, Guid>
    {
        public ContextoBD(DbContextOptions options)
            : base(options)
        {
        }
    }
}
