using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class EntityMap<TEntidad> : ClassMapping<TEntidad> where TEntidad : Entity
    {
        public EntityMap()
        {
            Id(c => c.Id, a => a.Generator(Generators.GuidComb));
            Property(c => c.Eliminado);
        }

        public void Enumerado<TEnumerado>(Expression<Func<TEntidad, TEnumerado>> propiedad)
        {
            Property(propiedad, a => a.Type<EnumStringType<TEnumerado>>());
        }

        public void Lista<TPropiedad>(string nombrePropiedad, string key, Action<IBagPropertiesMapper<TEntidad, TPropiedad>> actionMapper = null)
            where TPropiedad: Entity
        {
            Bag<TPropiedad>(nombrePropiedad, a =>
            {
                a.Key(k => k.Column(key));
                a.Cascade(Cascade.All);
                a.Type<ListaDiferidaFactory<TPropiedad>>();
                actionMapper?.Invoke(a);
            }, m => m.OneToMany());
        }

        public void Lista<TPropiedad>(Expression<Func<TEntidad, IEnumerable<TPropiedad>>> propiedad, string key,
          Action<IBagPropertiesMapper<TEntidad, TPropiedad>> actionMapper = null)
          where TPropiedad : Entity
        {
            Bag(propiedad, a =>
            {
                a.Key(k => k.Column(key));
                a.Cascade(Cascade.All);
                a.Type<ListaDiferidaFactory<TPropiedad>>();
                actionMapper?.Invoke(a);
            }, m => m.OneToMany());
        }
    }
}
