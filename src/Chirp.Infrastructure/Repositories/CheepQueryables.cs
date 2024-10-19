using Core.Entities;
using Core.Interfaces;
namespace Infrastructure.Services;

public static class QueryableExtensions
{

    private static readonly int _pageSize = 32;

    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page)
    {
        return
        source.Skip(Math.Max(0, page - 1) * _pageSize)
        .Take(_pageSize);
    }

    public static IEnumerable<Cheep> Search(this IEnumerable<Cheep> source, string searchQuery, Func<Cheep, string> fieldSelector)
    {
        return source.Where(a => fieldSelector(a).Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IEnumerable<Author> Search(this IEnumerable<Author> source, string searchQuery, Func<Author, string> fieldSelector)
    {
        return source.Where(a => fieldSelector(a).Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase));
    }
}
