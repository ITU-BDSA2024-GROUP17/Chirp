
using Chirp.Core.Entities;
using Chirp.Infrastructure.Extensions;
using Chirp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class AuthorRepository(CheepDbContext context) : IAuthorRepository
{
    private readonly CheepDbContext _context = context;

    public Task<List<Author>> GetAuthors(int page)
    {
        var authors = _context.Authors
            .Select(a => a)
            .OrderBy(a => a.UserName)
            .Paginate(page)
            .ToListAsync();

        return authors;
    }

    public Task<Author?> GetAuthor(string id)
    {
        var author = _context.Authors
            .Where(a => a.Id == id)
            .Include(a => a.Cheeps)
            .Include(a => a.Following)
            .Include(a => a.Followers)
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        return author;
    }

    public Task<Author?> GetAuthorByName(string name)
    {
        try
        {
            var author = _context.Authors.Where(a => a.UserName == name).First();
            return Task.FromResult<Author?>(author);
        }
        catch (InvalidOperationException)
        {
            return Task.FromResult<Author?>(null);
        }
    }

    public Task<List<Author>> SearchAuthors(string searchQuery, int page)
    {
        var authors = _context.Authors
            .Select(a => a)
            .Search(searchQuery, x => x.UserName ?? "")
            .OrderBy(a => a.UserName)
            .Paginate(page)
            .ToList();

        return Task.FromResult(authors);
    }

    public Task<List<Cheep>> GetCheeps(string author, int page)
    {
        var cheeps = _context.Authors
            .Where(a => a.UserName == author)
            .SelectMany(a => a.Cheeps)
            .Where(c => c.CheepOwnerId == null) // Should not include comments
            .Include(c => c.Revisions)
            .Include(c => c.Author)
            .Include(c => c.Likes)
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .AsSplitQuery()
            .OrderByDescending(c => c.Revisions.First().TimeStamp)
            .Paginate(page)
            .ToList();

        return Task.FromResult(cheeps);
    }


    public Task<int> GetCheepsCount(string author)
    {
        var cheeps = _context.Authors
            .Where(a => a.UserName == author)
            .SelectMany(a => a.Cheeps)
            .Where(c => c.CheepOwnerId == null) // Should not include comments
            .CountAsync();

        return cheeps;
    }

    public async Task<List<Cheep>> GetCheepsTimeline(string author, int page)
    {
        // Cheeps from author
        var authorCheeps = await _context.Authors
        .Where(a => a.UserName == author)
        .SelectMany(a => a.Cheeps)
        .Where(c => c.CheepOwnerId == null) // Should not include comments
        .Include(c => c.Author)
        .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
        .Include(c => c.Likes)
        .Include(c => c.Revisions)

        // Comment inclusion
        .Include(c => c.Comments).ThenInclude(c => c.Revisions)
        .Include(c => c.Comments).ThenInclude(c => c.Author).ThenInclude(a => a.Followers)
        .Include(c => c.Comments).ThenInclude(c => c.Likes)

        .AsSplitQuery()
        .ToListAsync();

        // Cheeps form follwing
        var followingCheeps = await _context.Authors
            .Where(a => a.UserName == author)
            .Include(a => a.Following)
            .SelectMany(a => a.Following.SelectMany(f => f.Cheeps))
            .Include(c => c.Author)
            .ThenInclude(a => a.Followers)
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Likes)
            .Include(c => c.Revisions)

            // Comment inclusion
            .Include(c => c.Comments).ThenInclude(c => c.Revisions)
            .Include(c => c.Comments).ThenInclude(c => c.Author).ThenInclude(a => a.Followers)
            .Include(c => c.Comments).ThenInclude(c => c.Likes)

            .AsSplitQuery()
            .ToListAsync();

        // Combine results
        var combinedCheeps = authorCheeps
            .Concat(followingCheeps)
            .OrderByDescending(c => c.Revisions.First().TimeStamp)
            .Paginate(page)
            .ToList();

        return combinedCheeps;
    }

    public Task<int> GetCheepsTimelineCount(string author)
    {
        var cheeps = _context.Authors
            .Where(a => a.UserName == author)
            .SelectMany(a => a.Cheeps)
            .Where(c => c.CheepOwnerId == null) // Should not include comments
            .Union(_context.Authors
                .Where(a => a.UserName == author)
                .Include(a => a.Following)
                .SelectMany(a => a.Following.SelectMany(f => f.Cheeps)))
                .Where(c => c.CheepOwnerId == null) // Should not include comments
            .CountAsync();

        return cheeps;
    }

    public Task<List<Cheep>> GetCheepsCommented(string authorId, int page)
    {
        var cheeps = _context.Cheeps

            .Where(c => c.CheepOwnerId == null) // Should not include comments
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Author).ThenInclude(a => a.Followers)
            .ThenInclude(a => a.Followers)
            .Include(c => c.Likes)

            // Comment inclusion
            .Include(c => c.Comments).ThenInclude(c => c.Revisions)
            .Include(c => c.Comments).ThenInclude(c => c.Author).ThenInclude(a => a.Followers)
            .Include(c => c.Comments).ThenInclude(c => c.Likes)

            .Where(c => c.Comments.Any(comment => comment.AuthorId == authorId))

            .OrderByDescending(c => c.Revisions.First().TimeStamp)
            .Paginate(page)
            .AsSplitQuery()
            .ToList();

        return Task.FromResult(cheeps);
    }

    public Task<int> GetCheepsCommentedCount(string authorId)
    {
        var count = _context.Cheeps
            .Include(c => c.Comments)
            .Where(c => c.Comments.Any(comment => comment.AuthorId == authorId))
            .CountAsync();

        return count;
    }


    public Task<List<Cheep>> GetLiked(string authorId, int page)
    {
        var cheeps = _context.Authors
            .Where(a => a.Id == authorId)
            .SelectMany(a => a.LikedCheeps)
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Author)
            .Include(c => c.Likes)
            .AsSplitQuery()
            .OrderByDescending(c => c.Revisions.First().TimeStamp)
            .Paginate(page)
            .ToList();

        return Task.FromResult(cheeps);
    }

    public Task<int> GetLikedCount(string authorId)
    {
        var cheeps = _context.Authors
            .Where(a => a.Id == authorId)
            .SelectMany(a => a.LikedCheeps)
            .CountAsync();

        return cheeps;
    }


    public Task<Author> GetAuthorByField(string author, Func<Author, string> field)
    {
        var FoundAuthor = _context.Authors
            .Search(author, field)
            .First();

        return Task.FromResult(FoundAuthor);
    }

    public Task<Author> CreateAuthor(Author author)
    {
        if (_context.Authors.Find(author.Id) != null) throw new InvalidDataException("Author is not avaliable");

        _context.Authors.Add(author);
        _context.SaveChanges();
        return Task.FromResult(author);
    }

    public Task<List<Author>> GetFollowing(string authorId)
    {
        var author = _context.Authors
            .Where(a => a.Id == authorId)
            .SelectMany(a => a.Following)
            .ToListAsync();

        return author;
    }

    public Task<List<Author>> GetFollowers(string authorId)
    {
        var author = _context.Authors
            .Where(a => a.Id == authorId)
            .SelectMany(a => a.Followers)
            .ToListAsync();

        return author;
    }

    public Task<Author> Follow(string followerId, string followeeId)
    {
        var follower = _context.Authors
            .Where(a => a.Id == followerId)
            .Include(a => a.Following)
            .FirstOrDefault() ?? throw new InvalidDataException("Author is not avaliable");
        var followee = _context.Authors.Find(followeeId) ?? throw new InvalidDataException("Author is not avaliable");

        if (follower.Following.Any(a => a.Id == followee.Id)) throw new Exception("Author is already followed!");
        follower.Following.Add(followee);

        _context.SaveChanges();

        return Task.FromResult(follower);
    }

    public Task<Author> Unfollow(string followerId, string followeeId)
    {
        var follower = _context.Authors
            .Where(a => a.Id == followerId)
            .Include(a => a.Following)
            .FirstOrDefault() ?? throw new InvalidDataException("Author is not avaliable");
        var followee = _context.Authors.Find(followeeId) ?? throw new InvalidDataException("Author is not avaliable");

        if (!follower.Following.Any(a => a.Id == followee.Id)) throw new Exception("Author is not following!");
        follower.Following.Remove(followee);

        _context.SaveChanges();

        return Task.FromResult(follower);
    }

    public Task<Author> UpdateAuthorPhoneNumber(string id, string phoneNumber)
    {
        var author = _context.Authors.Find(id) ?? throw new InvalidDataException("Author is not avaliable");

        author.PhoneNumber = phoneNumber;

        _context.SaveChanges();

        return Task.FromResult(author);
    }

    public Task<Author> UpdateAuthorAvatar(string id, string avatar)
    {
        var author = _context.Authors.Find(id) ?? throw new InvalidDataException("Author is not avaliable");

        author.Avatar = avatar;

        _context.SaveChanges();

        return Task.FromResult(author);
    }
}
