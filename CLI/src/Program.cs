using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;

if (args.Length == 0) return;

if (args[0] == "read")
{
    try
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);

        using var reader = new StreamReader("../chirp_cli_db.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Cheep>();

        foreach (var cheep in records)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);
            var time = offset.LocalDateTime;
            Console.WriteLine($"this person \"{cheep.Author}\" sent {cheep.Message} at: {time}");
        }
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
    }
}
else if (args[0] == "cheep")
{
    if (args.Length < 2)
    {
        Console.WriteLine("You have not written a cheep");
        return;
    }

    try
    {
        var records = new List<Cheep>
        {
            new() {
                Author = Environment.MachineName,
                Message = args[1].Trim(),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }
        };

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        using var stream = File.Open("../chirp_cli_db.csv", FileMode.Append);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, config);

        csv.WriteRecords(records);
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
    }
}
else
{
    Console.WriteLine("No recognized command, use \"cheep\" or \"read\"");
}