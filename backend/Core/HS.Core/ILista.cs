using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public interface ILista<T> : IList<T> where T : Entity
    {
        T Agregar(T entidad);
        Task<T> AgregarAsync(T entidad);
        void AgregarVarios(IEnumerable<T> entidades);
        T Buscar(Guid id);
        Task<T> BuscarAsync(Guid id);
        T Buscar(Expression<Func<T, bool>> expresion);
        Task<T> BuscarAsync(Expression<Func<T, bool>> expresion);
        S Buscar<S>(Guid id) where S : T;
        Task<S> BuscarAsync<S>(Guid id) where S : T;
        S Buscar<S>(Expression<Func<T, bool>> expresion) where S : T;
        Task<S> BuscarAsync<S>(Expression<Func<T, bool>> expresion) where S : T;
        IEnumerable<T> Filtrar(Expression<Func<T, bool>> expresion);
        Task<IEnumerable<T>> FiltrarAsync(Expression<Func<T, bool>> expresion);
        void ForEach(Action<T> accion);
    }
}
