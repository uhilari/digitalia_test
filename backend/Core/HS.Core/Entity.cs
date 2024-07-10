using System;

namespace HS
{
    public abstract class Entity
    {
        public virtual Guid Id { get; protected set; }
        public virtual bool Eliminado { get; protected set; }

        public virtual void Eliminar()
        {
            Eliminado = true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var entidad = obj as Entity;
            if (entidad == null)
                return false;
            if (Id == Guid.Empty && entidad.Id == Guid.Empty)
                return ReferenceEquals(this, entidad);

            return Id == entidad.Id;
        }

        public static bool operator ==(Entity izqda, Entity drcha)
        {
            if (object.ReferenceEquals(izqda, null))
            {
                return object.ReferenceEquals(drcha, null);
            }

            return izqda.Equals(drcha);
        }

        public static bool operator !=(Entity izqda, Entity drcha)
        {
            return !(izqda == drcha);
        }

        public static bool operator ==(Entity izqda, Guid id)
        {
            if (object.ReferenceEquals(izqda, null))
            {
                return false;
            }

            return izqda.Id == id;
        }

        public static bool operator !=(Entity izqda, Guid id)
        {
            return !(izqda == id);
        }
    }
}
