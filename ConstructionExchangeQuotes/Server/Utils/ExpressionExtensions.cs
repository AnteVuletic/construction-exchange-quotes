using System;
using System.Linq.Expressions;

namespace ConstructionExchangeQuotes.Server.Utils
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TEntity, bool>> AndAlso<TEntity>(this Expression<Func<TEntity, bool>> exprLeft, Expression<Func<TEntity, bool>> exprRight)
        {
            var parameter = Expression.Parameter(typeof(TEntity));

            var leftVisitor = new ReplaceExpressionVisitor(exprLeft.Parameters[0], parameter);
            var left = leftVisitor.Visit(exprLeft.Body);

            var rightVisitor = new ReplaceExpressionVisitor(exprRight.Parameters[0], parameter);
            var right = rightVisitor.Visit(exprRight.Body);

            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        private class ReplaceExpressionVisitor: ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}
