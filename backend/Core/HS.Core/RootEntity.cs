using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public class RootEntity: Entity
    {
        private IDataReader _reader;
        protected internal virtual IServiceProvider Provider { get; set; }
        protected virtual IDataReader Reader => _reader ?? (_reader = Provider.GetService<IDataReader>());

        protected IMaker<TSrc, TDst> GetMaker<TSrc, TDst>() where TSrc: class where TDst: class => Provider.GetService<IMapperFactory>().GetMaker<TSrc, TDst>();
    }
}
