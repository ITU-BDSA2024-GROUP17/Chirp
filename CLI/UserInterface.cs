using CLI.Records;

namespace CLI;

static class UserInterFace
{
    public static void PrintCheeps(List<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);
            var time = offset.LocalDateTime;

            Console.WriteLine($"{cheep.Author} @ {time}: {cheep.Message}");
        }
    }
}
