using AnchorModeling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AnchorModeling.QueryExtensions
{
    public static class AnchotSelectors
    {
        /// <summary>
        /// Selecting attributes of anchor
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isTemporary">select attribute marked by [Temporary]</param>
        /// <param name="attributeTableType">type of anchot attribute P_[H]_[EntityTable]_[AnchorAttribute]</param>
        /// <param name="attributePropertyInfo">propertyInfo of entity property</param>
        /// <param name="withTansaction"></param>
        /// <returns></returns>
        public static IQueryable<AnchorValue> SelectAnchorValues(object query,
        bool isTemporary,
            Type attributeTableType,
                PropertyInfo attributePropertyInfo,
                bool withTansaction)
        {
            Type resultType = typeof(AnchorValue);

            MethodInfo selectMethod = QueryMethods.SelectMethodInfo.MakeGenericMethod(attributeTableType, resultType);

            ParameterExpression parameter = Expression.Parameter(attributeTableType, "a");            

            List<MemberBinding> bindings = new List<MemberBinding>(){
                Expression.Bind(resultType.GetProperty(nameof(AnchorValue.AnchorId)), Expression.Property(parameter, Names.A_Id)),
                       Expression.Bind(resultType.GetProperty(nameof(AnchorValue.TransactionId)), Expression.Property(parameter, Names.TransactionIdFKName)),
                       Expression.Bind(resultType.GetProperty(nameof(AnchorValue.CloseTransactionId)), Expression.Property(parameter, Names.CloseTransactionIdFKName)),
                       Expression.Bind(resultType.GetProperty(nameof(AnchorValue.IsTemporary)), Expression.Constant(isTemporary, typeof(bool)))
                };

            if (isTemporary)
            {
                bindings.Add(Expression.Bind(resultType.GetProperty(nameof(AnchorValue.ApplicationTime)),
                    Expression.Property(parameter, Names.When)));
            }

            if (withTansaction)
            {
                bindings.AddRange(new[]{
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.Transaction)),Expression.Property(parameter, Names.TransactionFKPropName)),
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.CloseTransaction)), Expression.Property(parameter, Names.CloseTransactionFKPropName))
                    });
            }


            Type valueType = attributePropertyInfo.PropertyType;

            if (valueType.Equals(typeof(int)) || valueType.Equals(typeof(long))
                || valueType.Equals(typeof(byte)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.LongValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.LongValue)).PropertyType))
                    );
            }
            else
            if (valueType.Equals(typeof(float)) || valueType.Equals(typeof(double)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.DoubleValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.DoubleValue)).PropertyType))
                    );
            }
            else
            if (valueType.Equals(typeof(decimal)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.DecimalValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.DecimalValue)).PropertyType))
                    );
            }
            else
                if (valueType.Equals(typeof(string)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.StringValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.StringValue)).PropertyType))
                    );
            }
            else
                if (valueType.Equals(typeof(bool)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.BoolValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.BoolValue)).PropertyType))
                    );
            }
            else
                if (valueType.Equals(typeof(DateTime)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.TimeValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.TimeValue)).PropertyType))
                    );
            }
            else
                if (valueType.Equals(typeof(Guid)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.GuidValue)),
                       Expression.Convert(Expression.Property(parameter, Names.Value),
                       resultType.GetProperty(nameof(AnchorValue.GuidValue)).PropertyType))
                    );
            }
            else
            {
                throw new TypeNotFoundException(valueType.Name);
            }

            bindings.Add(Expression.Bind(resultType.GetProperty(nameof(AnchorValue.IsTie)),
                 Expression.Constant(false)));

            var body = Expression.MemberInit(Expression.New(resultType), bindings.ToArray());

            var lambda = Expression.Lambda(body, parameter);

            return (IQueryable<AnchorValue>)selectMethod.Invoke(null, new[] { query, lambda });
        }
    }
}