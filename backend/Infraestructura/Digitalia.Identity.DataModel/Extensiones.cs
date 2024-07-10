using Digitalia.Identity.Dominio;
using HS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.DataModel
{
    public static class Extensiones
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContextFactory<ContextoBD>(o => o.UseSqlServer(connectionString))
                .AddIdentityCore<Usuario>()
                .AddRoles<Perfil>()
                .AddEntityFrameworkStores<ContextoBD>()
                ;
            services.AddScoped<IContextFactory, ContextFactory<ContextoBD>>();

            return services;
        }
    }
}
