using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Aplicacion.Handlers;
using Digitalia.Identity.Aplicacion.Validators;
using Digitalia.Identity.Model;
using HS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion
{
    public static class Extensiones
    {
        static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services
                .AddCommand<RegistroCommand, RegistroCommandHandler>()
                .AddCommand<CreateTokenCommand, CreateTokenCommandHandler, CreateTokenResponse>()
                .AddCommand<UsuarioActualCommand, UsuarioActualCommandHandler, UsuarioActualResponse>()
                .AddCommand<ActualizaUsuarioCommand, ActualizaUsuarioCommandHandler>()
                .AddCommand<CambioPwdCommand, CambioPwdCommandHandler>();
        }

        static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<RegistroCommand>, RegistroCommandValidator>();
        }

        public static IServiceCollection AddAplicacion(this IServiceCollection services)
        {
            return services
                .AddCommands()
                .AddValidators();
        }
    }
}
