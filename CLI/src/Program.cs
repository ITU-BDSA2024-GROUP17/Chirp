#nullable enable
using System.Text.RegularExpressions;
using DocoptNet;


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

static int ShowHelp(string help) { Console.WriteLine(help); return 0; }
static int ShowVersion(string version) { Console.WriteLine(version); return 0; }
static int OnError(string usage) { Console.Error.WriteLine(usage); return 1; }

static int Run(IDictionary<string, ArgValue> arguments)
{
    if(arguments["read"].IsTrue){
        ShowCheeps();
    }
    if(arguments["cheep"].IsTrue && !string.IsNullOrEmpty(arguments["<message>"].ToString())){
        CheepCheep(arguments["<message>"].ToString());
    }
    return 0;
}

return parser.Parse(args) switch
{
    IArgumentsResult<IDictionary<string, ArgValue>> { Arguments: var arguments } => Run(arguments),
    IHelpResult => ShowHelp(usage),
    IVersionResult { Version: var version } => ShowVersion(version),
    IInputErrorResult { Usage: var use } => OnError(use),
    _ => throw new InvalidOperationException("Unexpected result type")
};



static int ShowCheeps() { 
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
     return 0; }


static int CheepCheep(string message) { 
    try
    {
        using (StreamWriter writer = new("./chirp_cli_db.csv", append: true))
        {
            writer.WriteLine($"\"{Environment.UserName}\",\"{message}\",{DateTimeOffset.Now.ToUnixTimeSeconds()}");
        }
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be written to:");
        Console.WriteLine(e.Message);
    }
    return 0; 
}