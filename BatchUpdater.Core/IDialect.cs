namespace BatchUpdater.Core
{
    public interface IDialect
    {
        string DefaultScheme { get; }
        string FormatTableName(TableInfo tableInfo);
        string FormatTableName(string tableName);
        string FormatColumnName(string propertyName);
        string FormatValue<TValue>(TValue value);
    }
}