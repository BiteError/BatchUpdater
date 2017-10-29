namespace BatchUpdater.Core
{
    public class DefaultDialect : IDialect
    {
        public string DefaultScheme => "";
        public string FormatTableName(TableInfo tableInfo)
        {
            return tableInfo.TableName;
        }

        public string FormatTableName(string tableName)
        {
            return tableName;
        }

        public string FormatColumnName(string propertyName)
        {
            return propertyName;
        }

        public string FormatValue<TValue>(TValue value)
        {
            return value.ToString();
        }
    }
}