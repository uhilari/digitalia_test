using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public static class TypeExtensions
    {
        public static bool IsEnumerable(this Type type)
        {
            var interfaceEnumerable = type.GetInterface("IEnumerable");
            return interfaceEnumerable != null;
        }

        public static bool IsAssignableClass(this Type type)
        {
            if (type == typeof(String))
                return false;
            if (type.IsEnumerable())
                return false;
            return type.IsClass;
        }

        public static Type GetGenArg(this Type type, int index = 0)
        {
            if (!type.IsGenericType)
                throw new InvalidOperationException();
            var genArgs = type.GetGenericArguments();
            if (genArgs.Length != 1)
                throw new InvalidOperationException();
            return genArgs[index];
        }
    }
}
