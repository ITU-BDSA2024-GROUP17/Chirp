namespace Web.Utilities;

public static class TimeUtilties
{
    private static readonly long unixHour = 3600;
    private static readonly long unixDay = 86400;
    private static readonly long unixYear = 31536000;

    public static string FormatDateTimePretty(DateTime timestamp)
    {
        long unixTimeStamp = ((DateTimeOffset)timestamp).ToUnixTimeSeconds();
        long TimeDiff = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - unixTimeStamp;

        if (TimeDiff < unixHour)
        {
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

    public static string FormatUnixTimeRaw(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).ToString("yyyy-MM-dd HH:mm:ss");
    }
}
