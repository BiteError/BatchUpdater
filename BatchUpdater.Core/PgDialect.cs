using System;

namespace BatchUpdater.Core
{
    public class PgDialect : IDialect
    {
        public string DefaultScheme => "public";

        public string FormatTableName(TableInfo tableInfo)
        {
            return $"{tableInfo.SchemeName}.\"{tableInfo.TableName}\"";
        }

        public string FormatTableName(string tableName)
        {
            return $"{DefaultScheme}.\"{tableName}\"";
        }

        public string FormatColumnName(string propertyName)
        {
            return $"\"{propertyName}\"";
        }

        public string FormatValue<TValue>(TValue value)
        {
            return IsNullable(value) && value == null
                ? "NULL"
                : ValueQuote(value, Format(value));
        }

        static bool IsNullable<TValue>(TValue value)
        {
            var type = typeof(TValue);
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        static string ValueQuote<TValue>(TValue value, string formattedValue)
        {
            if (value is string
                || value is DateTime
                || value is Guid)
            {
                return AddValueQuote(formattedValue);
            }

            return formattedValue;
        }

        static string AddValueQuote(string value)
        {
            return $"'{value}'";
        }
        static string Format<TValue>(TValue value)
        {
            if (value is string)
            {
                return value.ToString().Replace("'", "''");
            }
            if (value is decimal
                || value is double)
            {
                return value.ToString().Replace(",", ".");
            }
            if (value is DateTime)
            {
                return ((DateTime)(object)value).ToString(FormatConstants.DateIsoFormat).Replace(",", ".");
            }
            if (value is bool)
            {
                return value.ToString().ToUpper();
            }

            return value.ToString();
        }
    }
}