namespace CSVDBService.Records;

/// <summary>
/// A Cheep is a message posted by a user.
/// </summary>
/// <param name="Author">The author of the Cheep.</param>
/// <param name="Message">The message of the Cheep.</param>
/// <param name="Timestamp">The time the Cheep was posted.</param>
public record Cheep(string Author, string Message, long Timestamp);
