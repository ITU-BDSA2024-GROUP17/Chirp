using System.Text.RegularExpressions;

if (args.Length == 0) return;

if (args[0] == "read")
{
    try
    {
        List<Cheep> cheeps = [];

        using (StreamReader reader = new("./chirp_cli_db.csv"))
        {
            string? line;

            reader.ReadLine(); // Skip first line (headers of the CSV)

            while ((line = reader.ReadLine()) != null)
            {
                // Regex to split CSV into columns
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                // Separating row to separate parts
                string[] row = CSVParser.Split(line);

                var offset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(row[2]));
                var time = offset.LocalDateTime;
                cheeps.Add(new Cheep { Author = row[0], Message = row[1], Timestamp = time });
            }

            foreach (var cheep in cheeps)
            {
                Console.WriteLine($"this person \"{cheep.Author}\" sent {cheep.Message} at: {cheep.Timestamp}");
            }
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
        using (StreamWriter file = new("./chirp_cli_db.csv", append: true))
        {
            file.WriteLine($"{System.Environment.MachineName},\"{args[1].Trim()}\",{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            Console.WriteLine("Cheep cheeped!");
        }
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