using NHibernate;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class ProviderInterceptor: EmptyInterceptor, IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var result = base.OnLoad(entity, id, state, propertyNames, types);
            var rootEntity = entity as RootEntity;
            if (rootEntity != null)
            {
                Extensiones.SetProvider(rootEntity, _serviceProvider);
            }
            return result;
        }
    }
}
