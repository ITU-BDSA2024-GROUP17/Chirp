using Core.Entities;
using Infrastructure;

namespace Chirp.Infrastructure.Tests;

public static class DbInitializer
{
    public static void SeedDatabase(CheepDbContext cheepDbContext)
    {
        if (cheepDbContext.Authors.Any() && cheepDbContext.Cheeps.Any()) return;

        var a1 = new Author() { Id = 1, Name = "John Doe ", Email = "John-Doe@mail.com", Cheeps = [] };
        var a2 = new Author() { Id = 2, Name = "Jane Doe", Email = "Jane-Doe@mail.dk", Cheeps = [] };
        var a3 = new Author() { Id = 3, Name = "John Smith", Email = "John-Smith@mail.com", Cheeps = [] };
        var a4 = new Author() { Id = 3, Name = "Jane Smith", Email = "Jane-Smith@mail.com", Cheeps = [] };

        var authors = new List<Author>() { a1, a2, a3, a4 };

        var c1 = new Cheep() { Id = 1, AuthorId = a1.Id, Author = a1, Message = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { Id = 2, AuthorId = a2.Id, Author = a2, Message = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        var c3 = new Cheep() { Id = 3, AuthorId = a3.Id, Author = a3, Message = "In various enchanted attitudes, like the Sperm Whale.", TimeStamp = DateTime.Parse("2023-08-01 13:14:58") };
        var c4 = new Cheep() { Id = 4, AuthorId = a4.Id, Author = a4, Message = "Unless we succeed in establishing ourselves in some monomaniac way whatever significance might lurk in them.", TimeStamp = DateTime.Parse("2023-08-01 13:14:34") };
        var c5 = new Cheep() { Id = 5, AuthorId = a1.Id, Author = a1, Message = "At last we came back!", TimeStamp = DateTime.Parse("2023-08-01 13:14:35") };
        var c6 = new Cheep() { Id = 6, AuthorId = a2.Id, Author = a2, Message = "At first he had only exchanged one trouble for another.", TimeStamp = DateTime.Parse("2023-08-01 13:14:13") };
        var c7 = new Cheep() { Id = 7, AuthorId = a3.Id, Author = a3, Message = "In the first watch, and every creditor paid in full.", TimeStamp = DateTime.Parse("2023-08-01 13:16:13") };
        var c8 = new Cheep() { Id = 8, AuthorId = a4.Id, Author = a4, Message = "It was but a very ancient cluster of blocks generally painted green, and for no other, he shielded me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:01") };
        var c9 = new Cheep() { Id = 9, AuthorId = a1.Id, Author = a1, Message = "The folk on trust in me!", TimeStamp = DateTime.Parse("2023-08-01 13:15:30") };
        var c10 = new Cheep() { Id = 10, AuthorId = a2.Id, Author = a2, Message = "It is a damp, drizzly November in my pocket, and switching it backward and forward with a most suspicious aspect.", TimeStamp = DateTime.Parse("2023-08-01 13:13:34") };

        var cheeps = new List<Cheep> { c1, c2, c3, c4, c5, c6, c7, c8, c9, c10 };

        cheepDbContext.Authors.AddRange(authors);
        cheepDbContext.Cheeps.AddRange(cheeps);
        cheepDbContext.SaveChanges();
    }
}
