using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class CurrentScope : ICurrentScope
    {
        private readonly IHttpContextAccessor _serviceProvider;

        public CurrentScope(IHttpContextAccessor httpAccessor)
        {
            _serviceProvider = httpAccessor;
        }

        public T GetService<T>()
        {
            return (T)_serviceProvider.HttpContext.RequestServices.GetService(typeof(T));
        }

        public object GetService(Type type)
        {
            return _serviceProvider.HttpContext.RequestServices.GetService(type);
        }
    }
}
