using System.Linq.Expressions;
using System.Text;

namespace BatchUpdater.Core
{
    public class QueryBuilderVisitor<TEntity> : ExpressionVisitor
    {
        readonly StringBuilder builder = new StringBuilder();
        private readonly QueryBuilderConfig queryBuilderConfig;

        public QueryBuilderVisitor(QueryBuilderConfig queryBuilderConfig)
        {
            this.queryBuilderConfig = queryBuilderConfig;
        }

        public string GetQuery<TFunc>(Expression<TFunc> expression)
        {
            Visit(Evaluator.PartialEval(expression));
            return builder.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            builder.Append("(");

            VisitChild(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    builder.Append(" = ");
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    builder.Append(" AND ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    builder.Append(" OR ");
                    break;

                case ExpressionType.GreaterThan:
                    builder.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    builder.Append(" >= ");
                    break;

                case ExpressionType.LessThan:
                    builder.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    builder.Append(" <= ");
                    break;
            }

            VisitChild(node.Right);

            builder.Append(")");

            return node;
        }

        private void VisitChild(Expression node)
        {
            if (node.NodeType == ExpressionType.MemberAccess)
            {
                var name = ((MemberExpression) node).Member.Name;
                var formattedName = queryBuilderConfig.ColumnName<TEntity>(name);
                builder.Append(formattedName);
            }
            else
            {
                Visit(node);
            }
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var formattedValue = queryBuilderConfig.Dialect.FormatValue(node.Value);
            builder.Append(formattedValue);
            return node;
        }
    }
}