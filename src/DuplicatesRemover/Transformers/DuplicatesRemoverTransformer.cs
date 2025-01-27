using System.Text;
using Microsoft.Data.Analysis;

namespace DuplicatesRemover.Data.Transformers;

public class DuplicatesRemoverTransformer
{
    public static DataFrame RemoveDuplicates(DataFrame df, string[] onColumns)
    {
        HashSet<string> seenRows = new();
        List<DataFrameRow> uniqueRows = new();
        PrimitiveDataFrameColumn<bool> filterMask 
            = new ("Filter", new bool[df.Rows.Count]);
        for (var i = 0; i < df.Rows.Count; i++)
        {
            var row = df.Rows[i];
            var keyBuilder = new StringBuilder();
            foreach (var col in onColumns)
            {
                keyBuilder.Append(row[col].ToString()).Append('|');
            }
            var key = keyBuilder.ToString();
            filterMask[i] = seenRows.Add(key);
        }
        var clean = df.Filter(filterMask);
        return clean;
    }
}