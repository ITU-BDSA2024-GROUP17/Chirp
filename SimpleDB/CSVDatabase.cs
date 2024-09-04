namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader("../chirp_cli_db.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>();
        return records.ToList();
    }

    public void Store(T record)
    {
        var records = new List<T> { record };
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        using var stream = File.Open("../chirp_cli_db.csv", FileMode.Append);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, config);

        csv.WriteRecords(records);
    }
}