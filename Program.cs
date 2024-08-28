using System.Collections;
using System.Text.RegularExpressions;


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

class Cheep
{
    public string Author;
    public string Message;
    public DateTime Timestamp;
}