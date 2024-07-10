using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface IUpdater<TSrc, TDst>
        where TSrc: class
        where TDst: class
    {
        Task UpdateAsync(TSrc src, TDst dst);
    }
}
