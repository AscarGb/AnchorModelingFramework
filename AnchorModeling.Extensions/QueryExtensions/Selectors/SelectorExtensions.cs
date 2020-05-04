using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnchorModeling.QueryExtensions
{
    public static class SelectorExtensions
    {
        public static async Task<IEnumerable<AnchorValue>> SelectAsync<TEntity>(this AnchorModelSet<TEntity> set, bool WithTansaction = false, bool withTies = true)
        {
            var unionMethod = QueryMethods.UnionEnumerableMethodInfo.MakeGenericMethod(typeof(AnchorValue));

            IEnumerable<AnchorValue> resultQuery = null;

            foreach (var setData in set.AnchorSetDataList.AnchorProperties)
            {
                if (setData.IsTie && !withTies)
                    continue;

                Type tieValueType = null;

                IQueryable<AnchorValue> selectResult = !setData.IsTie ?
                    AnchotSelectors.SelectAnchorValues(
                    setData.Query,
                    setData.IsTemporary,
                    setData.TableType,
                    setData.AttributeProperty,
                    WithTansaction) :

                    AnchotTieSelectors.SelectTieAnchorValues(
                    setData.Query,
                    setData.IsTemporary,
                    setData.TableType,
                    setData.AttributeProperty,
                    WithTansaction, set.AnchorSetDataList.DbContext, out tieValueType);

                //https://github.com/dotnet/efcore/issues/18091
                var result = await selectResult.ToListAsync();

                result = result.Select(a =>
                {
                    var clone = (AnchorValue)a.Clone();
                    clone.AttributeType = setData.TableType;
                    clone.AnchorAttributePropertyInfo = setData.AttributeProperty;
                    if (tieValueType != null) clone.TieValueType = tieValueType;
                    return clone;
                }).ToList();

                if (resultQuery == null)
                    resultQuery = result;
                else
                    resultQuery = (IEnumerable<AnchorValue>)unionMethod.Invoke(null, new[] { resultQuery, result });

            }

            return resultQuery;
        }
    }
}
