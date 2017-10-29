using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BatchUpdater.Core
{
    internal static class ExpressionExtensions
    {
        public static string GetPropertyName<TEntity, TProperty>(this Expression<Func<TEntity, TProperty>> propertyLamda)
        {
            var memberSelectorExpression = propertyLamda.Body as MemberExpression;

            var property = memberSelectorExpression?.Member as PropertyInfo;

            if (property != null)
            {
                return property.Name;
            }
            else
            {
                throw new ArgumentException("Incorect property lambda", nameof(propertyLamda));
            }
        }
    }
}