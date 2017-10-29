namespace BatchUpdater.Core
{
    public interface IDefaultTableNameConvention
    {
        string TableName<TEntity>();
    }
}