using CLI;
using CLI.Records;
using DocoptNet;
using SimpleDB;

var db = CSVDatabase<Cheep>.Instance;


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
        List<Cheep> cheeps = getCheeps();
        UserInterFace.PrintCheeps(cheeps);
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
    IInputErrorResult => OnError(help),
    _ => throw new InvalidOperationException("Unexpected result type")
};

List<Cheep> getCheeps()
{
    var cheeps = db.Read();
    return cheeps.ToList();
}

void CheepCheep(string message)
{
    try
    {
        Cheep cheep = new(Environment.MachineName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        db.Store(cheep);
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be written to:");
        Console.WriteLine(e.Message);
    }
}
