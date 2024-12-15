using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Utilities;

public static class TimeUtilties
{
    public enum TimeSpanRoundType
    {
        Second,
        QuarterMinute,
        HalfMinute,
        Minute,
        QuarterHour,
        HalfHour,
        Hour,
    }

    public static TimeSpan Round(this TimeSpan span, TimeSpanRoundType type, MidpointRounding mode = MidpointRounding.ToEven) =>
        type switch
        {
            TimeSpanRoundType.Second => TimeSpan.FromSeconds(Math.Round(span.TotalSeconds)),
            TimeSpanRoundType.QuarterMinute => TimeSpan.FromSeconds(Math.Round(span.TotalSeconds / 15, 0, mode) * 15),
            TimeSpanRoundType.HalfMinute => TimeSpan.FromSeconds(Math.Round(span.TotalSeconds / 30, 0, mode) * 30),
            TimeSpanRoundType.Minute => TimeSpan.FromSeconds(Math.Round(span.TotalSeconds / 60, 0, mode) * 60),
            TimeSpanRoundType.QuarterHour => TimeSpan.FromMinutes(Math.Round(span.TotalMinutes / 15, 0, mode) * 15),
            TimeSpanRoundType.HalfHour => TimeSpan.FromMinutes(Math.Round(span.TotalMinutes / 30, 0, mode) * 30),
            TimeSpanRoundType.Hour => TimeSpan.FromMinutes(Math.Round(span.TotalMinutes / 60, 0, mode) * 60),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    public static TimeSpan DateTimeDiffFromNow(DateTime date, TimeSpanRoundType type)
    {
        return (DateTime.UtcNow - date).Round(type);
    }

    public static string FormatDateTimePretty(DateTime timestamp)
    {
        TimeSpan roundedDiff = DateTimeDiffFromNow(timestamp, TimeSpanRoundType.Minute);
        long timeDiffMinutes = (long)Math.Round(Math.Abs(roundedDiff.TotalMinutes), MidpointRounding.AwayFromZero);
        long timeDiffHours = (long)Math.Round(Math.Abs(roundedDiff.TotalHours), MidpointRounding.AwayFromZero);
        long timeDiffDays = (long)Math.Round(Math.Abs(roundedDiff.TotalDays), MidpointRounding.AwayFromZero);


        if (timeDiffMinutes < 60 && timeDiffHours == 0 && timeDiffDays == 0)
        {
            if (timeDiffMinutes <= 1)
            {
                return "Just now";
            }
            return $"{timeDiffMinutes} min{(timeDiffMinutes > 1 ? "s" : "")} ago";
        }
        else if (timeDiffHours < 24 && timeDiffDays == 0)
        {
            return $"{timeDiffHours} hour{(timeDiffHours > 1 ? "s" : "")} ago";
        }
        else if (timeDiffDays < 365)
        {
            return $"{timeDiffDays} day{(timeDiffDays > 1 ? "s" : "")} ago";
        }
        else
        {
            int years = (int)(timeDiffDays / 365);
            return $"Over {years} year{(years > 1 ? "s" : "")} ago";
        }
    }



    /// <summary>
    /// Turns a Unix timestamp into a formatted string.
    /// </summary>
    /// <param name="timestamp">The Unix timestamp to format</param>
    /// <returns>A pretty formatted string</returns>
    public static string FormatUnixTimeRaw(DateTime timestamp)
    {
        return timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
