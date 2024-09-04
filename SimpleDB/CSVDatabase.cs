namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string databasePath = "../chirp_cli_db.csv";

    private void CreateDatabaseFileIfMissing()
    {
        if (File.Exists(databasePath)) return;

        using var stream = File.Create(databasePath);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteHeader<T>();
        csv.NextRecord(); // Create a newline, such that new records wont be on headerline.
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        CreateDatabaseFileIfMissing();

        using var reader = new StreamReader(databasePath);
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

        CreateDatabaseFileIfMissing();

        using var stream = File.Open(databasePath, FileMode.Append);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, config);

        csv.WriteRecords(records);
    }
}
