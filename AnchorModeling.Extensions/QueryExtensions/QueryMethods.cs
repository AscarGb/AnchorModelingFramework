using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
    public static class QueryMethods
    {
        public static MethodInfo UnionMethodInfo { get; private set; }
        public static MethodInfo UnionEnumerableMethodInfo { get; private set; }
        public static MethodInfo SelectMethodInfo { get; private set; }
        public static MethodInfo WhereMethodInfo { get; private set; }
        public static MethodInfo OrderByDescendingMethodInfo { get; private set; }
        public static MethodInfo FirstOrDefaultMethodInfo { get; private set; }

        static QueryMethods()
        {
            UnionMethodInfo = FindUnionMethodInfo();
            SelectMethodInfo = FindSelectMethodInfo();
            UnionEnumerableMethodInfo = FindEnumerableUnionMethodInfo();
            WhereMethodInfo = FindWhereMethodInfo();
            OrderByDescendingMethodInfo = FindOrderByDescendingMethodInfo();
            FirstOrDefaultMethodInfo = FindFirstOrDefaultMethodInfo();
        }

        /// <summary>
        /// Find method
        /// public static IQueryable&lt;TSource&gt; Where&lt;TSource&gt;(this IQueryable&lt;TSource&gt; source, Expression&lt;Func&lt;TSource, bool&gt;&gt; predicate);
        /// </summary>
        /// <returns></returns>
        public static MethodInfo FindWhereMethodInfo() =>
            typeof(Queryable)
             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m =>
                         m.Name.Equals(nameof(Queryable.Where), StringComparison.Ordinal)
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 1
                            && m.GetParameters().Length == 2
                            && m.GetParameters()[1].ParameterType.GetGenericArguments()[0]
                                .GetGenericArguments().Length == 2);// Expression<Func<TSource, bool>> predicate         

        private static MethodInfo FindFirstOrDefaultMethodInfo() =>
            typeof(Queryable)
             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m =>
                    m.Name.Equals(nameof(Queryable.FirstOrDefault), StringComparison.Ordinal)
                           && m.IsGenericMethodDefinition
                           && m.GetGenericArguments().Length == 1
                           && m.GetParameters().Length == 1);

        private static MethodInfo FindOrderByDescendingMethodInfo() =>
            typeof(Queryable)
             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m =>
                        m.Name.Equals(nameof(Queryable.OrderByDescending), StringComparison.Ordinal)
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 2
                            && m.GetParameters().Length == 2);

        private static MethodInfo FindUnionMethodInfo() =>
            typeof(Queryable)
             .GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m =>
                    m.Name.Equals(nameof(Queryable.Union), StringComparison.Ordinal)
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 1
                            && m.GetParameters().Length == 2);

        private static MethodInfo FindEnumerableUnionMethodInfo() =>
            typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m =>
            m.Name.Equals(nameof(Queryable.Union), StringComparison.Ordinal)
                    && m.IsGenericMethodDefinition
                    && m.GetGenericArguments().Length == 1
                    && m.GetParameters().Length == 2);

        /// <summary>
        /// Find method
        /// public static IQueryable&lt;TResult&gt;  Select&lt;TSource, TResult&gt;(this IQueryable&lt;TSource&gt; source, Expression&lt;Func&lt;TSource, TResult&gt;&gt; selector)
        /// </summary>
        /// <returns></returns>
        public static MethodInfo FindSelectMethodInfo() =>
            typeof(Queryable)
             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m =>
                         m.Name.Equals(nameof(Queryable.Select), StringComparison.Ordinal)
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 2
                            && m.GetParameters().Length == 2
                            && m.GetParameters()[1].ParameterType.GetGenericArguments()[0]
                                .GetGenericArguments().Length == 2);
    }
}
