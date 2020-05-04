using AnchorModeling.Attributes;
using AnchorModeling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AnchorModeling.QueryExtensions
{
    public static class AnchotTieSelectors
    {
        public static IQueryable<AnchorValue> SelectTieAnchorValues(object query,
                bool isTemporary,
                    Type tieTableType,
                        PropertyInfo attributePropertyInfo,
                        bool WithTansaction,
                        DbContext context, out Type propType)
        {
            var contextType = context.GetType();
            var resultType = typeof(AnchorValue);
            var SelectMethod = QueryMethods.SelectMethodInfo.MakeGenericMethod(tieTableType, resultType);
            var parameter = Expression.Parameter(tieTableType, "a");
            var bindings = new List<MemberBinding>();

            BindBaseProperties(isTemporary, bindings, resultType, parameter);
            BindTemporay(isTemporary, bindings, resultType, parameter);
            BindTransactions(WithTansaction, bindings, resultType, parameter);
            BindLinkedAnchorId(bindings, resultType, parameter);

            var valuePropInfo = GetFirstLinkedByTieProperty(tieTableType, attributePropertyInfo);

            //имя типа для первого свойства узла
            var knotAttributeTable = valuePropInfo.GetFirstAttributeConstructorStringArgument(typeof(AttributeTypeAttribute));

            var attrKnotTableType = contextType.GetProperty(knotAttributeTable).PropertyType.GenericTypeArguments[0];

            propType = valuePropInfo.PropertyType;

            BindIsTie(bindings, resultType);

            var paramValueExpt = BuildTieValueExpression(attrKnotTableType, parameter, context);

            BindTieValue(paramValueExpt, propType, bindings, resultType);

            var Lambda = Expression.Lambda(
                Expression.MemberInit(Expression.New(resultType), bindings.ToArray()),
                parameter);

            return (IQueryable<AnchorValue>)SelectMethod.Invoke(null, new[] { query, Lambda });
        }

        private static PropertyInfo GetFirstLinkedByTieProperty(Type tieTableType, PropertyInfo attributePropertyInfo) =>
             tieTableType.Assembly.GetType(attributePropertyInfo.GetFirstAttributeConstructorStringArgument(typeof(TableTypeAttribute)), true, false)
                .GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(KnotValueAttribute)))
            ??
                 tieTableType.Assembly.GetType(attributePropertyInfo.GetFirstAttributeConstructorStringArgument(typeof(TableTypeAttribute)), true, false)
                .GetProperties()
                .First(p => p.IsDefined(typeof(AttributeTypeAttribute)));

        private static void BindTieValue(MemberExpression paramValueExpt, Type propType, List<MemberBinding> bindings, Type resultType)
        {
            if (propType.Equals(typeof(int)) || propType.Equals(typeof(long))
               || propType.Equals(typeof(byte)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.LongValue)),
                       Expression.Convert(paramValueExpt,
                       resultType.GetProperty(nameof(AnchorValue.LongValue)).PropertyType))
                    );
            }
            else
           if (propType.Equals(typeof(float)) || propType.Equals(typeof(double)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.DoubleValue)),
                       Expression.Convert(paramValueExpt, typeof(double)))
                    );
            }
            else
           if (propType.Equals(typeof(decimal)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.DecimalValue)),
                       Expression.Convert(paramValueExpt, typeof(decimal)))
                    );
            }
            else
               if (propType.Equals(typeof(string)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.StringValue)),
                       Expression.Convert(paramValueExpt, typeof(string)))
                    );
            }
            else
               if (propType.Equals(typeof(bool)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.BoolValue)),
                       Expression.Convert(paramValueExpt, typeof(bool)))
                    );
            }
            else
               if (propType.Equals(typeof(DateTime)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.TimeValue)),
                       Expression.Convert(paramValueExpt, typeof(DateTime)))
                    );
            }
            else
           if (propType.Equals(typeof(Guid)))
            {
                bindings.Add(
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.GuidValue)),
                        Expression.Convert(paramValueExpt, typeof(Guid)))
                    );
            }
            else
            {
                throw new TypeNotFoundException(propType.Name);
            }
        }

        private static MemberExpression BuildTieValueExpression(Type attrKnotTableType, ParameterExpression parameter, DbContext context)
        {
            var contextType = context.GetType();
            var parameterForAncAttr = Expression.Parameter(attrKnotTableType, "p");
            var parameterForOrder = Expression.Parameter(attrKnotTableType, "p1");
            var knotSetMethod = contextType.GetMethod(nameof(DbContext.Set)).MakeGenericMethod(attrKnotTableType);
            var FirstMethod = QueryMethods.FirstOrDefaultMethodInfo.MakeGenericMethod(attrKnotTableType);
            var OrderByDescendingMethod = QueryMethods.OrderByDescendingMethodInfo.MakeGenericMethod(attrKnotTableType, typeof(int));
            var WhereMethod = QueryMethods.WhereMethodInfo.MakeGenericMethod(attrKnotTableType);

            MemberExpression paramValueExpt = Expression.Property(
                    Expression.Call(null, FirstMethod,
                        Expression.Call(null, OrderByDescendingMethod,
                            Expression.Call(null, WhereMethod,
                               Expression.Call(Expression.Constant(context), knotSetMethod)//context.Set<P_Anchor_Name>()
                            , Expression.Lambda(Expression.Equal(
                                Expression.Property(parameterForAncAttr, Names.A_Id),
                                Expression.Property(parameter, Names.ToId)), parameterForAncAttr))//Where(p=>p.A_id == param.To_id)
                       , Expression.Lambda(Expression.Property(parameterForOrder, Names.TransactionIdFKName), parameterForOrder))//.OrderByDescending(u=>u.TrnId) 
                    ),//.FirstOrDefault()
                        Names.Value); //.Value

            return paramValueExpt;
        }

        private static void BindIsTie(List<MemberBinding> bindings, Type resultType)
        {
            bindings.Add(Expression.Bind(resultType.GetProperty(nameof(AnchorValue.IsTie)),
            Expression.Constant(true)));
        }

        private static void BindLinkedAnchorId(List<MemberBinding> bindings, Type resultType, ParameterExpression parameter) =>
            bindings.Add(Expression.Bind(resultType.GetProperty(nameof(AnchorValue.ToId)), Expression.Property(parameter, Names.ToId)));

        private static void BindBaseProperties(bool isTemporary, List<MemberBinding> bindings, Type resultType, ParameterExpression parameter)
        {
            bindings.AddRange(new[]{
                Expression.Bind(resultType.GetProperty(nameof(AnchorValue.AnchorId)), Expression.Property(parameter, Names.A_Id)),
                       Expression.Bind(resultType.GetProperty(nameof(AnchorValue.TransactionId)), Expression.Property(parameter, Names.TransactionIdFKName)),
                       Expression.Bind(resultType.GetProperty(nameof(AnchorValue.CloseTransactionId)), Expression.Property(parameter, Names.CloseTransactionIdFKName)),
                       Expression.Bind(resultType.GetProperty(nameof(AnchorValue.IsTemporary)), Expression.Constant(isTemporary, typeof(bool))),
                           });
        }

        private static void BindTemporay(bool isTemporary, List<MemberBinding> bindings, Type resultType, ParameterExpression parameter)
        {
            if (isTemporary)
            {
                bindings.Add(Expression.Bind(resultType.GetProperty(nameof(AnchorValue.ApplicationTime)),
                     Expression.Property(parameter, Names.When)
                     ));
            }
        }

        private static void BindTransactions(bool withTansaction, List<MemberBinding> bindings, Type resultType, ParameterExpression parameter)
        {
            if (withTansaction)
            {
                bindings.AddRange(new[]{
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.Transaction)),Expression.Property(parameter, Names.TransactionFKPropName)),
                    Expression.Bind(resultType.GetProperty(nameof(AnchorValue.CloseTransaction)), Expression.Property(parameter, Names.CloseTransactionFKPropName))
                    });
            }
        }
    }
}