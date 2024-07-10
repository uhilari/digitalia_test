using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class MapperFactory(IServiceProvider serviceProvider) : IMapperFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IMaker<TSrc, TDst> GetMaker<TSrc, TDst>()
            where TSrc : class
            where TDst : class
        {
            var mapper = _serviceProvider.GetService<IMaker<TSrc, TDst>>();
            if (mapper != null)
                return mapper;
            return new MapperBasic<TSrc, TDst>(_serviceProvider, this);
        }

        public IUpdater<TSrc, TDst> GetUpdater<TSrc, TDst>()
            where TSrc : class
            where TDst : class
        {
            throw new NotImplementedException();
        }
    }
}
