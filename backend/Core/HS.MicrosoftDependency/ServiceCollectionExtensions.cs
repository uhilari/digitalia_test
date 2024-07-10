using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HS
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Decorate<TInterface, TDecorator>(this IServiceCollection services)
          where TInterface : class
          where TDecorator : class, TInterface
        {
            var wrappedDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TInterface));

            if (wrappedDescriptor == null)
                throw new InvalidOperationException($"{typeof(TInterface).Name} is not registered");

            var objectFactory = ActivatorUtilities.CreateFactory(typeof(TDecorator), new[] { typeof(TInterface) });
            services.Replace(ServiceDescriptor.Describe(
              typeof(TInterface),
              s => (TInterface)objectFactory(s, new[] { s.CreateInstance(wrappedDescriptor) }),
              wrappedDescriptor.Lifetime)
            );

            return services;
        }

        public static IServiceCollection Decorate(this IServiceCollection services, Type tInterface, Type tDecorator)
        {
            var wrappedDescriptor = services.FirstOrDefault(s => s.ServiceType == tInterface);

            if (wrappedDescriptor == null)
                throw new InvalidOperationException($"{tInterface.Name} is not registered");

            var objectFactory = ActivatorUtilities.CreateFactory(tDecorator, new[] { tInterface });
            services.Replace(ServiceDescriptor.Describe(
              tInterface,
              s => objectFactory(s, new[] { s.CreateInstance(wrappedDescriptor) }),
              wrappedDescriptor.Lifetime)
            );

            return services;
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(services);

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
    }
}
