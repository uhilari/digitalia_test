using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HS.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
        where T : Entity
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public Specification<T> And(Specification<T> spec)
        {
            return new AndSpecification<T>(this, spec);
        }

        public Specification<T> Or(Specification<T> spec)
        {
            return new OrSpecification<T>(this, spec);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public static Specification<T> operator &(Specification<T> izqda, Specification<T> drcha)
        {
            if (izqda == null) throw new ArgumentNullException(nameof(izqda));
            if (drcha == null) throw new ArgumentNullException(nameof(drcha));

            return new AndSpecification<T>(izqda, drcha);
        }

        public static Specification<T> operator |(Specification<T> izqda, Specification<T> drcha)
        {
            if (izqda == null) throw new ArgumentNullException(nameof(izqda));
            if (drcha == null) throw new ArgumentNullException(nameof(drcha));

            return new OrSpecification<T>(izqda, drcha);
        }

        public static Specification<T> operator !(Specification<T> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));

            return new NotSpecification<T>(spec);
        }
    }
}
