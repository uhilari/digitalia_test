using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public interface ICurrentScope
    {
        T GetService<T>();
        object GetService(Type type);
    }
}
