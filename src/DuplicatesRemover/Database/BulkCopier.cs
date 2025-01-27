using System.Data;
using Microsoft.Data.Analysis;
using Microsoft.Data.SqlClient;

namespace DuplicatesRemover.Data.Database;

public class BulkCopier
{
    public static async Task BulkCopy(DataFrame data, string connectionString)
    {
        DataTable dataTable = data.ToTable();
        
        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using (SqlBulkCopy bulkCopy = new(connection, SqlBulkCopyOptions.CheckConstraints, null))
            {
                
                bulkCopy.DestinationTableName = "VentasTiendas";
                bulkCopy.ColumnMappings.Add("FECHA", "FECHA");
                bulkCopy.ColumnMappings.Add("TIENDA", "TIENDA");
                bulkCopy.ColumnMappings.Add("ARTICULO", "ARTICULO");
                bulkCopy.ColumnMappings.Add("VENTA", "VENTA");
                await bulkCopy.WriteToServerAsync(dataTable);
            }
        }
    }
}