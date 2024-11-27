namespace Chirp.Infrastructure.Extensions.Tests;

public class QueryableExtensionsTest
{
    [Fact]
    public void PaginateIEnumarableTest()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(i);
        }
        IEnumerable<int> Ienum = list;

        IEnumerable<int> returnedList = QueryableExtensions.Paginate(Ienum, 1);

        foreach (int nr in returnedList)
        {
            Assert.True(nr >= 0 && nr < 32);
        }

        Assert.Equal(32, returnedList.Count());
    }
}
