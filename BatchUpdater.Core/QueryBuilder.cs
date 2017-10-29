using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BatchUpdater.Core
{
    public class QueryBuilder<TEntity> where TEntity : class
    {
        public readonly StringBuilder Builder = new StringBuilder();
        public readonly List<string> ColumnUpdates = new List<string>();
        public Expression<Func<TEntity, bool>> WherePredicate;
        private readonly QueryBuilderConfig queryBuilderConfig;

        public QueryBuilder(QueryBuilderConfig queryBuilderConfig)
        {
            this.queryBuilderConfig = queryBuilderConfig;
        }

        public override string ToString()
        {
            return Builder.ToString();
        }

        
        public QueryBuilder<TEntity> Set<TValue>(Expression<Func<TEntity, TValue>> property, TValue propertyValue)
        {
            var propertyName = property.GetPropertyName();
            var formattedName = queryBuilderConfig.Dialect.FormatColumnName(propertyName);
            var formattedValue = queryBuilderConfig.Dialect.FormatValue(propertyValue);
            ColumnUpdates.Add($"{formattedName} = {formattedValue}");
            return this;
        }

        public QueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> wherePredicate)
        {
            WherePredicate = wherePredicate;
            return this;
        }

        public string GetUpdateQuery()
        {
            var tableName = queryBuilderConfig.TableName<TEntity>();
            Builder.Append($"UPDATE {tableName} SET ");

            var columnString = ColumnUpdates.Aggregate((x, y) => x + ", " + y);
            Builder.Append(columnString);


            var visitor = new QueryBuilderVisitor<TEntity>(queryBuilderConfig);
            var wherePredicate = visitor.GetQuery(WherePredicate);
            Builder.Append($" WHERE {wherePredicate};");
            return ToString();
        }
    }
}
