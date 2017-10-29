namespace BatchUpdater.Core
{
    public class DefaultTableNameConvention : IDefaultTableNameConvention
    {
        public string TableName<TEntity>()
        {
            return typeof(TEntity).Name;
        }
    }
}