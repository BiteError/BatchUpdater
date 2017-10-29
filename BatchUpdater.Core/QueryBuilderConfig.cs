using System;
using System.Collections.Generic;

namespace BatchUpdater.Core
{
    public class QueryBuilderConfig
    {
        public IDialect Dialect { get; private set; }
        IDefaultTableNameConvention defaultTableNameConvention = new DefaultTableNameConvention();
        Dictionary<Type, TableInfo> tableNames = new Dictionary<Type, TableInfo>();

        public QueryBuilderConfig WithDialect(IDialect dialect)
        {
            Dialect = dialect;
            return this;
        }

        public QueryBuilderConfig RegisterType<TEntity>(string tableName, string schemeName = null)
        {
            tableNames[typeof(TEntity)] = new TableInfo
            {
                TableName = tableName,
                SchemeName = schemeName
            };

            return this;
        }

        public string TableName<TEntity>()
        {
            var type = typeof(TEntity);

            if (tableNames.ContainsKey(type))
            {
                return Dialect.FormatTableName(tableNames[type]);
            }

            var tableName = defaultTableNameConvention.TableName<TEntity>();
            return Dialect.FormatTableName(tableName);
        }

        public string ColumnName<TEntity>(string propertyName)
        {
            return Dialect.FormatColumnName(propertyName);
        }
    }
}