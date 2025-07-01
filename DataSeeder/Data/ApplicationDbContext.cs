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

                Console.WriteLine($"Started Adding Data for Table: {destinationTable}");

                bulk.WriteToServer(table);

                Console.WriteLine($"Data Added Successfully for Table: {destinationTable}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB ERROR: {ex}");
            }
        }
    }
}