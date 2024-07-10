using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    internal class MapperBasic<TSrc, TDst>(IServiceProvider serviceProvider, MapperFactory mapperFactory) : IMaker<TSrc, TDst>, IUpdater<TSrc, TDst>
        where TSrc : class
        where TDst : class
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly MapperFactory _mapperFactory = mapperFactory;
        private IDataReader _reader = null;

        private IDataReader Reader
        {
            get
            {
                if (_reader == null)
                {
                    _reader = _serviceProvider.GetService<IDataReader>();
                }
                return _reader;
            }
        }

        private async Task CopiarPropiedades(TSrc src, TDst dst)
        {
            var propiedadesSrc = typeof(TSrc).GetProperties();
            var propiedadesDst = typeof(TDst).GetProperties();
            foreach (var propSrc in propiedadesSrc)
            {
                var propDst = propiedadesDst.FirstOrDefault(c => c.Name == propSrc.Name);
                if (propDst != null)
                {
                    if (propDst.PropertyType.IsAssignableClass() && propSrc.PropertyType.IsAssignableClass())
                    {
                        var getMakerGeneric = typeof(MapperFactory).GetMethod("GetMaker");
                        var getMaker = getMakerGeneric.MakeGenericMethod(propSrc.PropertyType, propDst.PropertyType);
                        var maker = (IMaker<TSrc, TDst>)getMaker.Invocar(_mapperFactory);
                        propDst.SetValue(dst, await maker.MakeAsync(src));
                    }
                    else if (propDst.PropertyType == typeof(Guid) && propSrc.PropertyType == typeof(string))
                    {
                        propDst.SetValue(dst, ((string)propSrc.GetValue(src)).Guid());
                    }
                    else if (propDst.PropertyType == typeof(string) && propSrc.PropertyType == typeof(Guid))
                    {
                        propDst.SetValue(dst, ((Guid)propSrc.GetValue(src)).Cadena());
                    }
                    else
                    {
                        propDst.SetValue(dst, propSrc.GetValue(src));
                    }
                }
                else
                {
                    propDst = propiedadesDst.FirstOrDefault(c => c.Name == $"Id{propSrc.Name}");
                    if (propDst != null && propSrc.DeclaringType.EsHijo(typeof(Entity)))
                    {
                        var entity = (Entity)propSrc.GetValue(src);
                        if (propDst.PropertyType ==  typeof(Guid))
                        {
                            propDst.SetValue(dst, entity.Id);
                        }
                        else if (propDst.PropertyType == typeof(string))
                        {
                            propDst.SetValue(dst, entity.Id.Cadena());
                        }
                    }
                    else
                    {
                        propDst = propiedadesDst.FirstOrDefault(c => $"Id{c.Name}" == propSrc.Name);
                        if (propDst != null && propDst.DeclaringType.EsHijo(typeof(Entity)))
                        {
                            Guid idSrc = Guid.Empty;
                            if (propSrc.PropertyType == typeof(Guid))
                            {
                                idSrc = (Guid)propSrc.GetValue(src);
                            }
                            else if (propSrc.PropertyType == typeof(string))
                            {
                                idSrc = ((string)propSrc.GetValue(src)).Guid();
                            }
                            var getEntityGeneric = Reader.GetType().GetMethod("GetAsync");
                            var getEntity = getEntityGeneric.MakeGenericMethod(propDst.PropertyType);
                            var entity = await (Task<TDst>)getEntity.Invoke(Reader, new object[] { idSrc });
                            propDst.SetValue(dst, entity);
                        }
                    }
                }
            }
        }

        public async Task<TDst> MakeAsync(TSrc src)
        {
            var dst = Activator.CreateInstance<TDst>();
            await CopiarPropiedades(src, dst);
            return dst;
        }

        public async Task UpdateAsync(TSrc src, TDst dst)
        {
            await CopiarPropiedades(src, dst);
        }
    }
}
