namespace Chirp.Web.Utilities;

public static class TimeUtilties
{
    private static readonly long unixMin = 60;
    private static readonly long unixHour = 3600;
    private static readonly long unixDay = 86400;
    private static readonly long unixYear = 31536000;

    /// <summary>
    /// Truns a DateTime object into a formatted string.
    /// </summary>
    /// <param name="timestamp">The DateTime timstamp to format</param>
    /// <returns>A pretty formated string</returns>
    public static string FormatDateTimePretty(DateTime timestamp)
    {
        long unixTimeStamp = ((DateTimeOffset)timestamp).ToUnixTimeSeconds();
        long TimeDiff = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - unixTimeStamp;

        if (TimeDiff < unixHour)
        {
            if (TimeDiff / unixMin > 1) // if a 2 mins has passed
            {
                return $"{TimeDiff / unixMin} mins ago";
            }
            return "Just now";
        }
        else if (TimeDiff < unixDay)
        {
            return $"{TimeDiff / unixHour} hours ago";
        }
        else if (TimeDiff < unixYear)
        {
            return $"{TimeDiff / unixDay} days ago";
        }
        else
        {
            return $"Over {TimeDiff / unixYear} years ago";
        }
    }

    /// <summary>
    /// Turns a Unix timestamp into a formatted string.
    /// </summary>
    /// <param name="timestamp">The Unix timestamp to format</param>
    /// <returns>A pretty formatted string</returns>
    public static string FormatUnixTimeRaw(DateTime timestamp)
    {
        return timestamp.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
