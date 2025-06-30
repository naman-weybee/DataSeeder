using Microsoft.Data.SqlClient;
using System.Data;

namespace DataSeeder.Data
{
    public static class ApplicationDbContext
    {
        private const string connectionString = "Server=DESKTOP-B8PQOC6;Database=ECommerce;TrustServerCertificate=True;Integrated Security=True;";

        public static void BulkInsert(DataTable table, string destinationTable)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                using var bulk = new SqlBulkCopy(connection)
                {
                    DestinationTableName = destinationTable
                };

                foreach (DataColumn column in table.Columns)
                    bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);

                bulk.WriteToServer(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}