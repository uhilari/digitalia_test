using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IEventContext: IDisposable
    {
        void Commit();
    }
}
