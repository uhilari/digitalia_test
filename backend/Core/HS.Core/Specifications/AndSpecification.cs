using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HS.Specifications
{
    public class AndSpecification<T> : Specification<T>
        where T: Entity
    {
        private readonly Specification<T> _izqda;
        private readonly Specification<T> _drcha;

        public AndSpecification(Specification<T> izqda, Specification<T> drcha)
        {
            _izqda = izqda;
            _drcha = drcha;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var izqdaExpr = _izqda.ToExpression();
            var drchaExpr = _drcha.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(izqdaExpr.Body, drchaExpr.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;
        }
    }
}
