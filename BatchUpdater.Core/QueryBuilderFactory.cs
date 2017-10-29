namespace BatchUpdater.Core
{
    public class QueryBuilderFactory
    {
        private QueryBuilderConfig queryBuilderConfig;

        public QueryBuilderFactory WithConfig(QueryBuilderConfig queryBuilderConfig)
        {
            this.queryBuilderConfig = queryBuilderConfig;
            return this;
        }

        public QueryBuilder<TEntity> Create<TEntity>() where TEntity : class
        {
            return new QueryBuilder<TEntity>(queryBuilderConfig);
        }
    }
}