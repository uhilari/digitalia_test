﻿using NHibernate;
using NHibernate.Collection.Generic;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    internal class ListaDiferida<T> : PersistentGenericBag<T>, ILista<T> where T : Entity
    {
        public ListaDiferida(ISessionImplementor session) : base(session) { }

        public ListaDiferida(ISessionImplementor session, IEnumerable<T> collection) : base(session, collection) { }

        public string RolLista { get; set; }

        private ICriteria _crearCriteria<S>() where S : T
        {
            var entry = this.Session.PersistenceContext.GetCollectionEntry(this);
            var persister = entry.LoadedPersister as OneToManyPersister;
            var criteria = new CriteriaImpl(typeof(S), Session);
            foreach (string columnName in persister.KeyColumnNames)
            {
                criteria.Add(NHibernate.Criterion.Expression.Sql(string.Format("this_.{0} = ?", columnName), Key, NHibernateUtil.Guid));
            }
            return criteria;
        }

        private S _Buscar<S>(Expression<Func<T, bool>> expresion, Action<ICriteria> agregarFiltro) where S : T
        {
            S entidad = null;
            if (base.WasInitialized)
            {
                entidad = this.FirstOrDefault(expresion.Compile()) as S;
            }
            else
            {
                var criteria = _crearCriteria<S>();
                agregarFiltro(criteria);
                entidad = criteria.UniqueResult<S>();
            }
            return entidad;
        }

        private async Task<S> _BuscarAsync<S>(Expression<Func<T, bool>> expresion, Action<ICriteria> agregarFiltro) where S : T
        {
            S entidad = null;
            if (base.WasInitialized)
            {
                entidad = this.FirstOrDefault(expresion.Compile()) as S;
            }
            else
            {
                var criteria = _crearCriteria<S>();
                agregarFiltro(criteria);
                entidad = await criteria.UniqueResultAsync<S>();
            }
            return entidad;
        }

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
            return _Buscar<T>(c => c.Id == id, cr =>
            {
                cr.Add(Restrictions.Eq("Id", id));
            });
        }

        public async Task<T> BuscarAsync(Guid id)
        {
            return await _BuscarAsync<T>(c => c.Id == id, cr =>
            {
                cr.Add(Restrictions.Eq("Id", id));
            });
        }

        public S Buscar<S>(Guid id) where S : T
        {
            return _Buscar<S>(c => c.Id == id, cr =>
            {
                cr.Add(Restrictions.Eq("Id", id));
            });
        }

        public async Task<S> BuscarAsync<S>(Guid id) where S : T
        {
            return await _BuscarAsync<S>(c => c.Id == id, cr =>
            {
                cr.Add(Restrictions.Eq("Id", id));
            });
        }

        public T Buscar(Expression<Func<T, bool>> expresion)
        {
            return _Buscar<T>(expresion, cr =>
            {
                cr.Add(Restrictions.Where(expresion));
            });
        }

        public async Task<T> BuscarAsync(Expression<Func<T, bool>> expresion)
        {
            return await _BuscarAsync<T>(expresion, cr =>
            {
                cr.Add(Restrictions.Where(expresion));
            });
        }

        public S Buscar<S>(Expression<Func<T, bool>> expresion) where S : T
        {
            return _Buscar<S>(expresion, cr =>
            {
                cr.Add(Restrictions.Where(expresion));
            });
        }

        public async Task<S> BuscarAsync<S>(Expression<Func<T, bool>> expresion) where S : T
        {
            return await _BuscarAsync<S>(expresion, cr =>
            {
                cr.Add(Restrictions.Where(expresion));
            });
        }

        public IEnumerable<T> Filtrar(Expression<Func<T, bool>> expresion)
        {
            IEnumerable<T> lista;
            if (base.WasInitialized)
            {
                lista = this.Where(expresion.Compile()).ToList();
            }
            else
            {
                var criteria = _crearCriteria<T>();
                criteria.Add(Restrictions.Where(expresion));
                lista = criteria.List<T>();
            }
            return lista;
        }

        public async Task<IEnumerable<T>> FiltrarAsync(Expression<Func<T, bool>> expresion)
        {
            IEnumerable<T> lista;
            if (base.WasInitialized)
            {
                lista = this.Where(expresion.Compile()).ToList();
            }
            else
            {
                var criteria = _crearCriteria<T>();
                criteria.Add(Restrictions.Where(expresion));
                lista = await criteria.ListAsync<T>();
            }
            return lista;
        }

        public void ForEach(Action<T> accion)
        {
            foreach (var item in this)
            {
                accion(item);
            }
        }
    }
}
