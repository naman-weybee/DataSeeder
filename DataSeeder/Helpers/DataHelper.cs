using System.Data;
using System.Reflection;

namespace DataSeeder.Helpers
{
    public static class DataHelper
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var table = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, propType);
            }

            foreach (var item in items)
            {
                var values = props.Select(p => p.GetValue(item) ?? DBNull.Value).ToArray();
                table.Rows.Add(values);
            }

            return table;
        }
    }
}