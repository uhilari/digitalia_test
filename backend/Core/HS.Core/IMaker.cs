using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IMaker<TSrc, TDst> 
        where TSrc: class
        where TDst: class
    {
        Task<TDst> MakeAsync(TSrc src);
    }
}
