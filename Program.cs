using System.Collections;
using System.Net;
using System.Text.RegularExpressions;

if (args.Length == 0) return;

if (args[0] == "read")
{
    try
    {
        List<Cheep> cheeps = [];

        using StreamReader reader = new("./chirp_cli_db.csv");
        string? line;

        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            //Separating columns to array
            string[] X = CSVParser.Split(line);

            var offset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(X[2]));
            var time = offset.LocalDateTime;
            cheeps.Add(new Cheep {Author = X[0], Message = X[1], Timestamp = time});

        }

        foreach (var cheep in cheeps)
        {
            // Console.WriteLine(cheep.Author + " " + cheep.Message + " " + cheep.Timestamp);
            Console.WriteLine($"this person \"{cheep.Author}\" sent {cheep.Message} at: {cheep.Timestamp}");
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
        StreamWriter file = new("./chirp_cli_db.csv", append: true);
        file.WriteLine($"{System.Environment.MachineName},\"{args[1].Trim()}\",{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
        file.Close();
        Console.WriteLine("Cheep cheeped!");
    }
    catch (IOException e) {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
    }
}
else
{
    Console.WriteLine("No recognized command, use \"cheep\" or \"read\"");
}

class Cheep
{
    public required string Author;
    public required string Message;
    public required DateTime Timestamp;
}