using Chirp.Web.Utilities;

namespace Chirp.Web.Tests.Util;

public class FormattersTest
{
    static readonly DateTime now = DateTime.UtcNow;

    public static readonly TheoryData<DateTime> CasesStatic =
    new()
    {
        { DateTime.UnixEpoch },
        { new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
    };

    public static readonly TheoryData<int, DateTime> CasesDynamic =
    new()
    {
        // 3 mins past
        { 0, now.AddSeconds(15) },
        { 1, now.AddMinutes(3) },
        { 2, now.AddHours(2) },
        { 3, now.AddDays(4) },
        { 4, now.AddYears(1) },
    };

    [Theory, MemberData(nameof(CasesStatic))]
    public void FormatUnixTimeTest(DateTime UnixTimeStamp)
    {
        string t = TimeUtilties.FormatUnixTimeRaw(UnixTimeStamp);
        Assert.Equal("1970-01-01 00:00:00", t);
        Assert.NotEqual("1970-01-01 00:00:01", t);
    }

    [Theory, MemberData(nameof(CasesDynamic))]
    public void FormatDateTimePrettyTest(int index, DateTime date)
    {
        string prettyTest = TimeUtilties.FormatDateTimePretty(date);
        switch (index)
        {
            case 0:
                Assert.InRange(TimeUtilties.DateTimeDiffFromNow(date, TimeUtilties.TimeSpanRoundType.Minute).TotalSeconds, -2.0, 2.0);
                Assert.Equal("Just now", prettyTest);
                break;
            case 1:
                Assert.InRange(TimeUtilties.DateTimeDiffFromNow(date, TimeUtilties.TimeSpanRoundType.Minute).TotalMinutes, -3.0, -2.0);
                Assert.Equal("3 mins ago", prettyTest);
                break;

            case 2:
                Assert.InRange(TimeUtilties.DateTimeDiffFromNow(date, TimeUtilties.TimeSpanRoundType.Hour).TotalHours, -2.0, -1.0);
                Assert.Equal("2 hours ago", prettyTest);
                break;
            case 3:
                Assert.Equal(-4, TimeUtilties.DateTimeDiffFromNow(date, TimeUtilties.TimeSpanRoundType.Hour).TotalDays);
                Assert.Equal("4 days ago", prettyTest);
                break;
            case 4:
                Assert.Equal(-1, TimeUtilties.DateTimeDiffFromNow(date, TimeUtilties.TimeSpanRoundType.Hour).TotalDays / 365);
                Assert.Equal("Over 1 year ago", prettyTest);
                break;
        }
    }

}
