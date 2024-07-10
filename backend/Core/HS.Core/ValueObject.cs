using System;
using System.Collections.Generic;
using System.Text;

namespace HS
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetValues();
        public virtual bool Equals(T obj)
        {
            if (obj is null)
                return false;
            var thsValues = GetValues().GetEnumerator();
            var objValues = obj.GetValues().GetEnumerator();
            while (thsValues.MoveNext() && objValues.MoveNext())
            {
                if (thsValues.Current is null ^ objValues.Current is null)
                    return false;
                if (thsValues.Current != null && !thsValues.Current.Equals(objValues.Current))
                    return false;
            }
            return !thsValues.MoveNext() && !objValues.MoveNext();
        }

        public override bool Equals(object obj)
        {
            var objectValue = obj as T;
            if (objectValue is null)
                return false;
            return Equals(objectValue);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ValueObject<T> izqda, T drcha)
        {
            if (izqda is null)
            {
                return drcha is null;
            }
            return izqda.Equals(drcha);
        }

        public static bool operator!=(ValueObject<T> izqda, T drcha)
        {
            return !(izqda == drcha);
        }
    }
}
