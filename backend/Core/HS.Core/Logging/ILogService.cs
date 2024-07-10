using System;
using System.Reflection;

namespace HS.Logging
{
    public interface ILogService
    {
        void Detalle(Type clase, MethodInfo metodo, object[] parametros, object resultado);
        void Error(Type clase, MethodInfo metodo, object[] parametros, Exception error);
    }
}
