using System;
using System.Linq;
using System.Linq.Expressions;

namespace AnchorModeling.QueryExtensions
{
    public static class ConditionExtensions
    {
        public static AnchorModelSet<TEntity> Where<TEntity>(this AnchorModelSet<TEntity> set, Expression<Func<SharedAnchorModel, bool>> expr)
        {
            foreach (var setData in set.AnchorSetDataList.AnchorProperties)
            {
                try
                {
                    var whereMethod = QueryMethods.WhereMethodInfo.MakeGenericMethod(setData.TableType);

                    var parameter = Expression.Parameter(setData.TableType, "a");

                    var whereExpr = Expression.Lambda(
                        new ExpressionParameterModifier<SharedAnchorModel>().Modify(expr.Body, parameter),
                        parameter);

                    setData.Query = (IQueryable)whereMethod.Invoke(null, new object[] { setData.Query, whereExpr });
                }
                catch (ArgumentException)
                {
                    //TODO : write to serilog
                }
            }

            return set;
        }
    }
}
