using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public class Lista<T> : List<T>, ILista<T> where T : Entity
    {
        public T Agregar(T entidad)
        {
            entidad.NoEsNull(nameof(entidad));
            base.Add(entidad);
            return entidad;
        }

        public Task<T> AgregarAsync(T entidad) => Task.Run(() =>
        {
            entidad.NoEsNull(nameof(entidad));
            base.Add(entidad);
            return entidad;
        });

        public void AgregarVarios(IEnumerable<T> entidades)
        {
            foreach (var item in entidades)
            {
                Agregar(item);
            }
        }

        public Task AgregarVariosAsync(IEnumerable<T> entidades) => Task.Run(() =>
        {
            foreach (var item in entidades)
            {
                Agregar(item);
            }
        });

        public T Buscar(Guid id)
        {
            return Buscar(c => c.Id == id);
        }

        public async Task<T> BuscarAsync(Guid id)
        {
            return await BuscarAsync(c => c.Id == id);
        }

        public T Buscar(Expression<Func<T, bool>> expresion)
        {
            expresion.NoEsNull(nameof(expresion));
            return this.FirstOrDefault(expresion.Compile());
        }

        public Task<T> BuscarAsync(Expression<Func<T, bool>> expresion) => Task.Run(() =>
        {
            expresion.NoEsNull(nameof(expresion));
            return this.FirstOrDefault(expresion.Compile());
        });

        public S Buscar<S>(Guid id) where S : T
        {
            return (S)Buscar(id);
        }

        public async Task<S> BuscarAsync<S>(Guid id) where S : T
        {
            return (S)await BuscarAsync(id);
        }

        public S Buscar<S>(Expression<Func<T, bool>> expresion) where S : T
        {
            return (S)Buscar(expresion);
        }

        public async Task<S> BuscarAsync<S>(Expression<Func<T, bool>> expresion) where S : T
        {
            return (S)await BuscarAsync(expresion);
        }

        public IEnumerable<T> Filtrar(Expression<Func<T, bool>> expresion)
        {
            expresion.NoEsNull(nameof(expresion));
            return this.Where(expresion.Compile()).ToList();
        }

        public Task<IEnumerable<T>> FiltrarAsync(Expression<Func<T, bool>> expresion) => Task.Run<IEnumerable<T>>(() =>
        {
            expresion.NoEsNull(nameof(expresion));
            return this.Where(expresion.Compile()).ToList();
        });
    }
}
