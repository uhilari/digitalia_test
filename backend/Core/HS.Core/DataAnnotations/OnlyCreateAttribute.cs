using System;
using System.Collections.Generic;
using System.Text;

namespace HS.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class OnlyCreateAttribute : Attribute
    {
    }
}
