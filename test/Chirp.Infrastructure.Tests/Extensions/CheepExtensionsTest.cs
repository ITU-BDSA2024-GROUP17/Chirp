using Chirp.Infrastructure.Extensions;

namespace Chirp.Infrastructure.Tests.Extensions;

public class CheepExtensionsTest
{
    [Test]
    public void PaginateIEnumarableTest()
    {
        List<int> list = [];
        for (int i = 0; i < 100; i++)
        {
            list.Add(i);
        }
        IEnumerable<int> Ienum = list;

        IEnumerable<int> returnedList = QueryableExtensions.Paginate(Ienum, 1);

        foreach (int nr in returnedList)
        {
            Assert.That(nr, Is.GreaterThanOrEqualTo(0));
            Assert.That(nr, Is.LessThan(32));
        }

        Assert.That(returnedList.Count(), Is.EqualTo(32));
    }
}
