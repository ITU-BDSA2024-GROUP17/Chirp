using System.Text.RegularExpressions;
using DocoptNet;
using SimpleDB;

var db = new CSVDatabase<Cheep>();


const string help = @"chirp.
Usage:
   read               Read all cheeps
   cheep <message>    Cheep a message
Options:
    -h --help     Show this screen.
    --version     Show version.
";

const string usage = @"chirp.
Usage:
   chirp read 
   chirp cheep <message>
   chirp (-h | --help)
   chirp --version
Options:
    -h --help     Show this screen.
    --version     Show version.
";


var parser = Docopt.CreateParser(usage).WithVersion("chirp 0.1");

int ShowHelp(string help) { Console.WriteLine(help); return 0; }
int ShowVersion(string version) { Console.WriteLine(version); return 0; }
int OnError(string usage) { Console.Error.WriteLine(usage); return 1; }

int Run(IDictionary<string, ArgValue> arguments)
{
    if (arguments["read"].IsTrue)
    {
        ShowCheeps();
    }
    if (arguments["cheep"].IsTrue && !string.IsNullOrEmpty(arguments["<message>"].ToString()))
    {
        CheepCheep(arguments["<message>"].ToString());
    }
    return 0;
}

return parser.Parse(args) switch
{
    IArgumentsResult<IDictionary<string, ArgValue>> { Arguments: var arguments } => Run(arguments),
    IHelpResult => ShowHelp(help),
    IVersionResult { Version: var version } => ShowVersion(version),
    IInputErrorResult { Usage: var use } => OnError(use),
    _ => throw new InvalidOperationException("Unexpected result type")
};



int ShowCheeps()
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
    return 0;
}


int CheepCheep(string message)
{
    try
    {
        Cheep cheep = new()
        {
            Author = Environment.MachineName,
            Message = message,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
        db.Store(cheep);
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be written to:");
        Console.WriteLine(e.Message);
    }
    return 0;
}