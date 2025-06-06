using System.Data;

namespace InvoiceSyncTool.Utilities
{
    public class InvoiceToDataTable
    {
        public static DataTable? Execute<T>(IEnumerable<T> data, params string[] excludedColumns)
        {
            DataTable table = new();

            if (data == null || !data.Any())
                return null;

            var properties = typeof(T).GetProperties()
                .Where(p => !excludedColumns.Contains(p.Name));

            foreach (var prop in properties)
            {
                var columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, columnType);
            }

            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item) ?? DBNull.Value).ToArray();
                table.Rows.Add(values);
            }

            return table;
        }
    }
}