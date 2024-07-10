using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public interface IMapperFactory
    {
        IMaker<TSrc, TDst> GetMaker<TSrc, TDst>()
            where TSrc : class
            where TDst : class;

        IUpdater<TSrc, TDst> GetUpdater<TSrc, TDst>()
            where TSrc : class
            where TDst : class;
    }
}
