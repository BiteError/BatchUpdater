using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BatchUpdater.Core
{
    public class QueryBuilder<TEntity, TKey> where TEntity : class
    {
        public readonly StringBuilder Builder = new StringBuilder();
        public readonly TEntity Entity;
        public readonly Expression<Func<TEntity, TKey>> IdProperty;
        public readonly List<string> ColumnUpdates = new List<string>();

        public QueryBuilder(TEntity entity, Expression<Func<TEntity, TKey>> idProperty)
        {
            Entity = entity;
            IdProperty = idProperty;
        }

        public override string ToString()
        {
            return Builder.ToString();
        }        
    }

    public static class QueryBuilderExtensions
    {
        public static QueryBuilder<TEntity, TKey> Update<TEntity, TKey>(this TEntity entity, Expression<Func<TEntity, TKey>> idProperty) where TEntity : class
        {
            return new QueryBuilder<TEntity, TKey>(entity, idProperty);
        }

        public static QueryBuilder<TEntity, TKey> WithTableName<TEntity, TKey>(this QueryBuilder<TEntity, TKey> builder, string name) where TEntity : class
        {
            builder.Builder.Append($"UPDATE dbo.\"{name}\" SET ");
            return builder;
        }

        public static QueryBuilder<TEntity, TKey> WithProperty<TEntity, TKey>(
            this QueryBuilder<TEntity, TKey> builder, Expression<Func<TEntity, double?>> property) where TEntity : class
        {
            var propertyName = property.GetPropertyName();
            var propertyValue = property.GetPropertyValueFrom(builder.Entity);
            builder.ColumnUpdates.Add($"\"{propertyName}\" = {propertyValue.FormatParam()}");
            return builder;
        }

        public static QueryBuilder<TEntity, TKey> WithProperty<TEntity, TKey>(
            this QueryBuilder<TEntity, TKey> builder, Expression<Func<TEntity, bool>> property) where TEntity : class
        {
            var propertyName = property.GetPropertyName();
            var propertyValue = property.GetPropertyValueFrom(builder.Entity);
            builder.ColumnUpdates.Add($"\"{propertyName}\" = {propertyValue.FormatParam()}");
            return builder;
        }

        public static QueryBuilder<TEntity, TKey> WithProperty<TEntity, TKey>(
            this QueryBuilder<TEntity, TKey> builder, Expression<Func<TEntity, string>> property) where TEntity : class
        {
            var propertyName = property.GetPropertyName();
            var propertyValue = property.GetPropertyValueFrom(builder.Entity);
            builder.ColumnUpdates.Add($"\"{propertyName}\" = {propertyValue.FormatParam()}");
            return builder;
        }

        static string FormatParam(this string value)
        {
            return value == null ? "NULL" : $"'{value.Replace("'", "''")}'";

        }

        static string FormatParam(this double? value)
        {
            return value == null ? "NULL" : value.ToString().Replace(",", ".");
        }

        static string FormatParam(this bool value)
        {
            return value.ToString().ToUpper();
        }

        public static TProperty GetPropertyValueFrom<Entity, TProperty>(this Expression<Func<Entity, TProperty>> propertyLamda, Entity entity)
        {
            var memberSelectorExpression = propertyLamda.Body as MemberExpression;

            var property = memberSelectorExpression?.Member as PropertyInfo;

            if (property != null)
            {
                return (TProperty)property.GetValue(entity);
            }
            else
            {
                throw new ArgumentException("Incorect property lambda", nameof(propertyLamda));
            }
        }

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

        public static string GetQuery<TEntity, TKey>(
            this QueryBuilder<TEntity, TKey> builder) where TEntity : class
        {
            var idValue = builder.IdProperty.GetPropertyValueFrom(builder.Entity);
            var columnString = builder.ColumnUpdates.Aggregate((x, y) => x + ", " + y);
            builder.Builder.Append(columnString);
            builder.Builder.Append($" WHERE \"Id\" = '{idValue}';");
            return builder.ToString();
        }
    }
}
