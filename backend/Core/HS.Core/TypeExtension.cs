using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HS
{
    public static class TypeExtension
    {
        public static string GetPropName<T, S>(this Expression<Func<T, S>> expression)
        {
            Type t = typeof(T);

            if (expression.Body is MemberExpression member)
            {
                if (member.Member is PropertyInfo propInfo)
                {
                    return propInfo.Name;
                }
            }
            return null;
        }
    }
}
