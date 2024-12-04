using Chirp.Core.Interfaces;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class CheepRepository(CheepDbContext context) : ICheepRepository
{
    private readonly CheepDbContext _context = context;

    public Task<List<Cheep>> GetCheeps(int page)
    {
        Task<List<Cheep>> cheeps = _context.Cheeps
            .Where(c => c.CheepOwnerId == null)
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Author).ThenInclude(a => a.Followers)
            .ThenInclude(a => a.Followers)
            .Include(c => c.Likes)

            // Comment inclusion
            .Include(c => c.Comments).ThenInclude(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Comments).ThenInclude(c => c.Author).ThenInclude(a => a.Followers)
            .Include(c => c.Comments).ThenInclude(c => c.Likes)

            .AsSplitQuery()
            .OrderByDescending(c => c.Revisions.First().TimeStamp)
            .Paginate(page)
            .ToListAsync();

        return cheeps;
    }

    public Task<int> CountCheeps()
    {
        var cheeps = _context.Cheeps.CountAsync();

        return cheeps;
    }

    public Task<Cheep?> GetCheep(int id)
    {
        var cheep = _context.Cheeps
            .Include(c => c.Author)
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Likes)
            .AsSplitQuery()
            .FirstOrDefault(c => c.Id == id);

        return Task.FromResult(cheep);
    }

    public Task<List<Cheep>> SearchCheeps(string searchQuery, int page)
    {
        var cheeps = _context.Cheeps
            .Where(c => c.CheepOwnerId == null)
            .Include(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Likes)

            // Cheep.Comments inclusion
            .Include(c => c.Comments).ThenInclude(c => c.Revisions.OrderByDescending(r => r.TimeStamp))
            .Include(c => c.Comments).ThenInclude(c => c.Author).ThenInclude(a => a.Followers)
            .Include(c => c.Comments).ThenInclude(c => c.Likes)

            .AsSplitQuery()
            .Search(searchQuery, x => x.Revisions.First().Message)
            .OrderByDescending(c => c.Revisions.Last().TimeStamp)
            .Paginate(page)
            .ToList();

        return Task.FromResult(cheeps);
    }

    public async Task<Cheep> CreateCheep(Cheep cheep)
    {
        if (new List<CheepRevision>(cheep.Revisions)[0].Message.Length > 160) throw new InvalidDataException("Message is too long");
        _context.Cheeps.Add(cheep);
        await _context.SaveChangesAsync();

        return cheep;
    }

    public Task<Cheep> UpdateCheep(int cheepId, CheepRevision cheepRevision)
    {
        var cheep = _context.Cheeps.Find(cheepId) ?? throw new InvalidDataException("Cheep does not exist!");

        List<CheepRevision> tempList = cheep.Revisions.ToList();
        tempList.Add(cheepRevision);
        cheep.Revisions = tempList;

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }

    public Task<Cheep> LikeCheep(int cheepId, string authorId)
    {
        var cheep = _context.Cheeps.Find(cheepId) ?? throw new InvalidDataException("Cheep does not exist!");
        var author = _context.Authors.Find(authorId) ?? throw new InvalidDataException("Author does not exist!");

        if (cheep.Likes.Contains(author)) throw new Exception("Cheep already liked!");

        cheep.Likes.Add(author);

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }

    public Task<Cheep> UnlikeCheep(int cheepId, string authorId)
    {
        var cheep = _context.Cheeps.Find(cheepId) ?? throw new InvalidDataException("Cheep does not exist!");
        var author = _context.Authors.Find(authorId) ?? throw new InvalidDataException("Author does not exist!");

        if (!cheep.Likes.Contains(author)) throw new Exception("Cheep not liked!");

        cheep.Likes.Remove(author);

        _context.SaveChanges();

        return Task.FromResult(cheep);
    }


    public async Task PostComment(int CheepToCommentId, Cheep comment)
    {
        if (new List<CheepRevision>(comment.Revisions)[0].Message.Length > 160) throw new InvalidDataException("Comment is too long");

        var cheepToComment = await GetCheep(CheepToCommentId) ?? throw new Exception("Cheep not found to comment!");
        cheepToComment.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCheep(int cheepId)
    {
        var cheepToDelete = _context.Cheeps
            .Where(c => c.Id == cheepId)
            .Include(c => c.Likes)
            .Include(c => c.Revisions)
            .Include(c => c.Comments)
            .AsSplitQuery()
            .FirstOrDefault() ?? throw new Exception("Cheep not found for delete");

        var commentIds = cheepToDelete.Comments.Select(c => c.Id).ToList();

        foreach (var commentId in commentIds)
        {
            await DeleteCheep(commentId);
        }

        _context.Cheeps.Remove(cheepToDelete);
        await _context.SaveChangesAsync();
    }
}
