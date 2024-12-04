using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Extensions;

public static class QueryableExtensions
{
    private static readonly int _pageSize = 32;

    /// <summary>
    /// Paginates an IQueryable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source IQueryable collection.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>An IQueryable collection containing the elements of the specified page.</returns>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page)
    {
        return source.Skip(Math.Max(0, page - 1) * _pageSize).Take(_pageSize);
    }

    /// <summary>
    /// Paginates an IEnumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source IEnumerable collection.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>An IEnumerable collection containing the elements of the specified page.</returns>
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page)
    {
        return source.Skip(Math.Max(0, page - 1) * _pageSize).Take(_pageSize);
    }

    /// <summary>
    /// Searches an IEnumerable collection of Cheeps.
    /// </summary>
    /// <param name="source">The source IEnumerable collection of Cheeps.</param>
    /// <param name="searchQuery">The search query string.</param>
    /// <param name="fieldSelector">A function to select the field to search in each Cheep.</param>
    /// <returns>An IEnumerable collection of Cheeps that match the search query.</returns>
    public static IEnumerable<Cheep> Search(this IEnumerable<Cheep> source, string searchQuery, Func<Cheep, string> fieldSelector)
    {
        return source.Where(a => fieldSelector(a).Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// Searches an IEnumerable collection of Authors.
    /// </summary>
    /// <param name="source">The source IEnumerable collection of Authors.</param>
    /// <param name="searchQuery">The search query string.</param>
    /// <param name="fieldSelector">A function to select the field to search in each Author.</param>
    /// <returns>An IEnumerable collection of Authors that match the search query.</returns>
    public static IEnumerable<Author> Search(this IEnumerable<Author> source, string searchQuery, Func<Author, string> fieldSelector)
    {
        return source.Where(a => fieldSelector(a).Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase));
    }
}
