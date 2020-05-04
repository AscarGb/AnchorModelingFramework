using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
    public class ExpressionParameterModifier<T> : ExpressionVisitor
    {
        private ParameterExpression _parameter;

        public Expression Modify(Expression expression, ParameterExpression parameter)
        {
            _parameter = parameter;
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node) => _parameter;

        protected override Expression VisitMember(MemberExpression node)
        {           
            if (node.Expression?.Type.Equals(typeof(T)) ?? false)
            {
                var attr = node.Member.GetCustomAttribute<ColumnAttribute>();
                var name = attr?.Name ?? node.Member.Name;
                return Expression.Property(base.Visit(node.Expression), name);
            }

            return base.VisitMember(node);
        }
    }
}
