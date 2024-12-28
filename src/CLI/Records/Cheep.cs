namespace CLI.Records;

/// <summary>
/// A record representing a message.
/// The messages are made by users and saved in the database.
/// </summary>
/// <param name="Author">Name of the author that wrote the message.</param>
/// <param name="Message">Text in the message.</param>
/// <param name="Timestamp">Unix time of when the message was made.</param>
public record Cheep(string Author, string Message, long Timestamp);
