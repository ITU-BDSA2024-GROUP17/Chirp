using System.Net.Http.Headers;
using System.Net.Http.Json;
using CLI;
using CLI.Records;
using DocoptNet;

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

var baseURL = "http://localhost:5281"; // This should use the correct URL based on the environment
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);

var parser = Docopt.CreateParser(usage).WithVersion("chirp 0.1");

int ShowHelp(string help) { Console.WriteLine(help); return 0; }
int ShowVersion(string version) { Console.WriteLine(version); return 0; }
int OnError(string usage) { Console.Error.WriteLine(usage); return 1; }

async Task<int> Run(IDictionary<string, ArgValue> arguments)
{
    if (arguments["read"].IsTrue)
    {
        List<Cheep> cheeps = await getCheepsAsync();
        UserInterFace.PrintCheeps(cheeps);
    }
    if (arguments["cheep"].IsTrue && !string.IsNullOrEmpty(arguments["<message>"].ToString()))
    {
        await postCheepAsync(arguments["<message>"].ToString());
    }

    return 0;
}

return parser.Parse(args) switch
{
    IArgumentsResult<IDictionary<string, ArgValue>> { Arguments: var arguments } => await Run(arguments),
    IHelpResult => ShowHelp(help),
    IVersionResult { Version: var version } => ShowVersion(version),
    IInputErrorResult => OnError(help),
    _ => throw new InvalidOperationException("Unexpected result type")
};

async Task<List<Cheep>> getCheepsAsync()
{
    var cheeps = await client.GetFromJsonAsync<List<Cheep>>("cheeps");

    return cheeps ?? [];
}

async Task postCheepAsync(string message)
{

    Cheep cheep = new(Environment.MachineName, message, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

    await client.PostAsJsonAsync("/cheep", cheep);
}

// Branch protection test
