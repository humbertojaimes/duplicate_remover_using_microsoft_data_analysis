using System.Data;
using System.Diagnostics;
using Microsoft.Data.Analysis;
using Microsoft.Extensions.Configuration;

namespace DuplicatesRemover.Data;

public class SalesService(IConfiguration configuration) 
{
    public async Task ProcessData()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        var salesData = await ExtractData(configuration["SalesDataPath"]);
        Console.WriteLine($"Data extracted with {salesData.Rows.Count} rows");
        var transformedData = await  TransformData(salesData);
        Console.WriteLine($"Data transformed with {transformedData.Rows.Count} rows");
        await LoadData(transformedData);
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
    }
    
    private Task<DataFrame> ExtractData(string path)
    {
        var salesData = File.ReadAllBytes(path);
        DataFrame frame = DataFrame.LoadCsv(new MemoryStream(salesData), 
            columnNames:["FECHA", "TIENDA", "ARTICULO","VENTA"] 
            ,dataTypes:[typeof(string),typeof(int),typeof(int),typeof(int)]);
        return Task.FromResult(frame);
    }

    private Task<DataFrame> TransformData(DataFrame data)
    {
        var cleaned = Transformers.DuplicatesRemoverTransformer
            .RemoveDuplicates(data,["FECHA", "TIENDA", "ARTICULO","VENTA"]);
        return Task.FromResult(cleaned);
    }

    private async Task LoadData(DataFrame data)
    {
        await Database.BulkCopier.BulkCopy(data,configuration.GetConnectionString("SalesDb"));
    }
    
    
}