using SimpleDB;

var db = new CSVDatabase<Cheep>();

if (args.Length == 0) return;

if (args[0] == "read")
{
    try
    {
        var cheeps = db.Read();

        foreach (var cheep in cheeps.ToList())
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
        Cheep cheep = new() {
            Author = Environment.MachineName,
            Message = args[1].Trim(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
        db.Store(cheep);
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