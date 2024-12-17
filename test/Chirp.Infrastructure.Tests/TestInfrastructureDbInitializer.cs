using Chirp.Core.Entities;

namespace Chirp.Infrastructure.Tests;

public static class TestInfrastructureDbInitializer
{
    public static void SeedDatabase(CheepDbContext cheepDbContext)
    {
        if (cheepDbContext.Authors.Any() || cheepDbContext.Cheeps.Any()) return;

        var a1 = new Author() { Id = "2bcf724c-b650-476c-ae11-d408eb2105a0", UserName = "John Doe", Email = "John-Doe@mail.com", Cheeps = [] };
        var a2 = new Author() { Id = "ac71fc84-62b9-4907-9b90-5305e1e25c96", UserName = "Jane Doe", Email = "Jane-Doe@mail.dk", Cheeps = [] };
        var a3 = new Author() { Id = "797776e2-0ac8-4493-a9dd-526a24146a87", UserName = "John Smith", Email = "John-Smith@mail.com", Cheeps = [], Following = [a1] };
        var a4 = new Author() { Id = "5ba7ed22-77c3-43da-899c-ac31e563d036", UserName = "Jane Smith", Email = "Jane-Smith@mail.com", Cheeps = [] };

        var authors = new List<Author>() { a1, a2, a3, a4 };

        // With Like
        var c1 = new Cheep() { Id = 1, AuthorId = a1.Id, Author = a1, Likes = new List<Author>() { a2 }, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") } } };

        // With Comment
        var c2 = new Cheep() { Id = 2, AuthorId = a2.Id, Author = a2, Comments = new List<Cheep>() { c1 }, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") } } };

        // With Like (count = 2) and Comments (count = 2)
        var c3 = new Cheep() { Id = 3, AuthorId = a3.Id, Author = a3, Comments = new List<Cheep>() { c1, c2 }, Likes = new List<Author>() { a1, a2 }, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "In various enchanted attitudes, like the Sperm Whale.", TimeStamp = DateTime.Parse("2023-08-01 13:14:58") } } };

        // With Like (count = 3), Comment (count = 2) and Revisions (count = 2)
        var c4 = new Cheep() { Id = 4, AuthorId = a4.Id, Author = a4, Comments = new List<Cheep>() { c1, c2 }, Likes = new List<Author>() { a1, a2, a3 }, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Unless we succeed in establishing ourselves in some monomaniac way whatever significance might lurk in them.", TimeStamp = DateTime.Parse("2023-08-01 13:14:34") }, new CheepRevision() { Message = "Unless we succeed in establishing ourselves in some monomaniac way whatever significance might lurk in them.", TimeStamp = DateTime.Parse("2023-08-01 13:16:34") } } };

        //
        // FOLLOWING

        /// Boiler plate

        var c5 = new Cheep() { Id = 5, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "At last we came back!", TimeStamp = DateTime.Parse("2023-08-01 13:14:35") } } };
        var c6 = new Cheep() { Id = 6, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "At first he had only exchanged one trouble for another.", TimeStamp = DateTime.Parse("2023-08-01 13:14:13") } } };
        var c7 = new Cheep() { Id = 7, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "In the first watch, and every creditor paid in full.", TimeStamp = DateTime.Parse("2023-08-01 13:16:13") } } };
        var c8 = new Cheep() { Id = 8, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "It was but a very ancient cluster of blocks generally painted green, and for no other, he shielded me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:01") } } };
        var c9 = new Cheep() { Id = 9, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "The folk on trust in me!", TimeStamp = DateTime.Parse("2023-08-01 13:15:30") } } };
        var c10 = new Cheep() { Id = 10, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "It is a damp, drizzly November in my pocket, and switching it backward and forward with a most suspicious aspect.", TimeStamp = DateTime.Parse("2023-08-01 13:13:34") } } };
        var c11 = new Cheep() { Id = 11, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I had no difficulty in finding where Sholto lived, and take it and in Canada.", TimeStamp = DateTime.Parse("2023-08-01 13:14:11") } } };
        var c12 = new Cheep() { Id = 12, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "What did they take?", TimeStamp = DateTime.Parse("2023-08-01 13:14:44") } } };
        var c13 = new Cheep() { Id = 13, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "It struck cold to see you, Mr. White Mason, to our shores a number of young Alec.", TimeStamp = DateTime.Parse("2023-08-01 13:13:23") } } };
        var c14 = new Cheep() { Id = 14, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "You are here for at all?", TimeStamp = DateTime.Parse("2023-08-01 13:13:18") } } };
        var c15 = new Cheep() { Id = 15, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "My friend took the treasure-box to the window.", TimeStamp = DateTime.Parse("2023-08-01 13:15:17") } } };
        var c16 = new Cheep() { Id = 16, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "But ere I could not find it a name that I come from.", TimeStamp = DateTime.Parse("2023-08-01 13:17:18") } } };
        var c17 = new Cheep() { Id = 17, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Then Sherlock looked across at the window, candle in his wilful disobedience of the road.", TimeStamp = DateTime.Parse("2023-08-01 13:14:30") } } };
        var c18 = new Cheep() { Id = 18, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "The message was as well live in this way-- SHERLOCK HOLMES--his limits.", TimeStamp = DateTime.Parse("2023-08-01 13:13:40") } } };
        var c19 = new Cheep() { Id = 19, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I commend that fact very carefully in the afternoon.", TimeStamp = DateTime.Parse("2023-08-01 13:13:20") } } };
        var c20 = new Cheep() { Id = 20, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "In the card-case is a wonderful old man!", TimeStamp = DateTime.Parse("2023-08-01 13:15:42") } } };
        var c21 = new Cheep() { Id = 21, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "But this is his name! said Holmes, shaking his hand.", TimeStamp = DateTime.Parse("2023-08-01 13:13:21") } } };
        var c22 = new Cheep() { Id = 22, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "She had turned suddenly, and a lady who has satisfied himself that he has heard it.", TimeStamp = DateTime.Parse("2023-08-01 13:15:51") } } };
        var c23 = new Cheep() { Id = 23, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "You were dwelling upon the ground, the sky, the spray that he would be a man''s forefinger dipped in blood.", TimeStamp = DateTime.Parse("2023-08-01 13:13:55") } } };
        var c24 = new Cheep() { Id = 24, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Mrs. Straker tells us that his mates thanked God the direful disorders seemed waning.", TimeStamp = DateTime.Parse("2023-08-01 13:14:00") } } };
        var c25 = new Cheep() { Id = 25, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I don''t like it, he said, and would have been just a little chat with me.", TimeStamp = DateTime.Parse("2023-08-01 13:13:59") } } };
        var c26 = new Cheep() { Id = 26, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "With back to my friend, patience!", TimeStamp = DateTime.Parse("2023-08-01 13:16:58") } } };
        var c27 = new Cheep() { Id = 27, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Is there a small outhouse which stands opposite to me, so as to my charge.", TimeStamp = DateTime.Parse("2023-08-01 13:14:38") } } };
        var c28 = new Cheep() { Id = 28, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I was too crowded, even on a leaf of my adventures, and had a license for the gallows.", TimeStamp = DateTime.Parse("2023-08-01 13:13:35") } } };
        var c29 = new Cheep() { Id = 29, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "A draghound will follow aniseed from here to enter into my heart.", TimeStamp = DateTime.Parse("2023-08-01 13:14:38") } } };
        var c30 = new Cheep() { Id = 30, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "That is where the wet and shining eyes.", TimeStamp = DateTime.Parse("2023-08-01 13:13:27") } } };
        var c31 = new Cheep() { Id = 31, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "If thou speakest thus to me that it was most piteous, that last journey.", TimeStamp = DateTime.Parse("2023-08-01 13:14:34") } } };
        var c32 = new Cheep() { Id = 32, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "My friend, said he.", TimeStamp = DateTime.Parse("2023-08-01 13:13:36") } } };
        var c33 = new Cheep() { Id = 33, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "He laid an envelope which was luxurious to the back part of their coming.", TimeStamp = DateTime.Parse("2023-08-01 13:13:58") } } };
        var c34 = new Cheep() { Id = 34, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Leave your horses below and nerving itself to concealment.", TimeStamp = DateTime.Parse("2023-08-01 13:16:54") } } };
        var c35 = new Cheep() { Id = 35, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Still, there are two brave fellows! Ha, ha!", TimeStamp = DateTime.Parse("2023-08-01 13:13:51") } } };
        var c36 = new Cheep() { Id = 36, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Well, Mr. Holmes, but glanced with some confidence, that the bed beside him.", TimeStamp = DateTime.Parse("2023-08-01 13:13:18") } } };
        var c37 = new Cheep() { Id = 37, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "But I have quite come to Mackleton with me now for a small figure, sir.", TimeStamp = DateTime.Parse("2023-08-01 13:15:23") } } };
        var c38 = new Cheep() { Id = 38, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Every word I say to them ahead, yet with their fists and sticks.", TimeStamp = DateTime.Parse("2023-08-01 13:13:39") } } };
        var c39 = new Cheep() { Id = 39, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "A well-fed, plump Huzza Porpoise will yield you about saying, sir?", TimeStamp = DateTime.Parse("2023-08-01 13:13:32") } } };
        var c40 = new Cheep() { Id = 40, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Holmes glanced at his busy desk, hurriedly making out his watch, and ever afterwards are missing, Starbuck!", TimeStamp = DateTime.Parse("2023-08-01 13:13:26") } } };
        var c41 = new Cheep() { Id = 41, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Like household dogs they came at last come for you.", TimeStamp = DateTime.Parse("2023-08-01 13:14:16") } } };
        var c42 = new Cheep() { Id = 42, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "To him it had done a great fish to swallow up the steel head of the cetacea.", TimeStamp = DateTime.Parse("2023-08-01 13:17:10") } } };
        var c43 = new Cheep() { Id = 43, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Thence he could towards me.", TimeStamp = DateTime.Parse("2023-08-01 13:13:23") } } };
        var c44 = new Cheep() { Id = 44, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "There was still asleep, she slipped noiselessly from the shadow lay upon the one that he was pretty clear now.", TimeStamp = DateTime.Parse("2023-08-01 13:14:14") } } };
        var c45 = new Cheep() { Id = 45, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Of course, it instantly occurred to him, whom all thy creativeness mechanical.", TimeStamp = DateTime.Parse("2023-08-01 13:13:25") } } };
        var c46 = new Cheep() { Id = 46, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "And you''ll probably find some other English whalers I know nothing of my revolver.", TimeStamp = DateTime.Parse("2023-08-01 13:15:09") } } };
        var c47 = new Cheep() { Id = 47, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "His necessities supplied, Derick departed; but he rushed at the end of the previous night.", TimeStamp = DateTime.Parse("2023-08-01 13:13:49") } } };
        var c48 = new Cheep() { Id = 48, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "We will leave the metropolis at this point of view you will do good by stealth.", TimeStamp = DateTime.Parse("2023-08-01 13:13:59") } } };
        var c49 = new Cheep() { Id = 49, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "One young fellow in much the more intimate acquaintance.", TimeStamp = DateTime.Parse("2023-08-01 13:15:23") } } };
        var c50 = new Cheep() { Id = 50, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "The shores of the middle of it, and you can imagine, it was probable, from the hall.", TimeStamp = DateTime.Parse("2023-08-01 13:14:10") } } };
        var c51 = new Cheep() { Id = 51, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "His bridle is missing, so that a dangerous man to be that they had been employed between 8.30 and the boat to board and lodging.", TimeStamp = DateTime.Parse("2023-08-01 13:16:19") } } };
        var c52 = new Cheep() { Id = 52, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "The room into which one hopes.", TimeStamp = DateTime.Parse("2023-08-01 13:13:19") } } };
        var c53 = new Cheep() { Id = 53, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "The area before the fire until he broke at clapping, as at Coxon''s.", TimeStamp = DateTime.Parse("2023-08-01 13:15:10") } } };
        var c54 = new Cheep() { Id = 54, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "There he sat; and all he does not use his powers of observation and deduction.", TimeStamp = DateTime.Parse("2023-08-01 13:16:38") } } };
        var c55 = new Cheep() { Id = 55, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Mr. Thaddeus Sholto WAS with his methods of work, Mr. Mac.", TimeStamp = DateTime.Parse("2023-08-01 13:15:23") } } };
        var c56 = new Cheep() { Id = 56, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "The commissionnaire and his hands to unconditional perdition, in case he was either very long one.", TimeStamp = DateTime.Parse("2023-08-01 13:14:22") } } };
        var c57 = new Cheep() { Id = 57, AuthorId = a2.Id, Author = a2, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "See how that murderer could be from any trivial business not connected with her.", TimeStamp = DateTime.Parse("2023-08-01 13:13:21") } } };
        var c58 = new Cheep() { Id = 58, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I was asking for your lives!''  _Wharton the Whale Killer_.", TimeStamp = DateTime.Parse("2023-08-01 13:13:35") } } };
        var c59 = new Cheep() { Id = 59, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Besides,'' thinks I, ''it was only a simple key?", TimeStamp = DateTime.Parse("2023-08-01 13:13:38") } } };
        var c60 = new Cheep() { Id = 60, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I thought that you are bored to death in the other.", TimeStamp = DateTime.Parse("2023-08-01 13:16:13") } } };
        var c61 = new Cheep() { Id = 61, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "D''ye see him? cried Ahab, exultingly but on!", TimeStamp = DateTime.Parse("2023-08-01 13:15:13") } } };
        var c62 = new Cheep() { Id = 62, AuthorId = a3.Id, Author = a3, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "I think, said he, Holmes, with all hands to stand on!", TimeStamp = DateTime.Parse("2023-08-01 13:14:50") } } };
        var c63 = new Cheep() { Id = 63, AuthorId = a4.Id, Author = a4, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "It came from a grove of Scotch firs, and I were strolling on the soft gravel, and finally the dining-room.", TimeStamp = DateTime.Parse("2023-08-01 13:14:04") } } };
        var c64 = new Cheep() { Id = 64, AuthorId = a1.Id, Author = a1, Revisions = new List<CheepRevision>() { new CheepRevision() { Message = "Nor can piety itself, at such a pair of as a lobster if he had needed it; but no, it''s like that, does he?", TimeStamp = DateTime.Parse("2023-08-01 13:15:42") } } };

        var cheeps = new List<Cheep> { c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17, c18, c19, c20, c21, c22, c23, c24, c25, c26, c27, c28, c29, c30, c31, c32, c33, c34, c35, c36, c37, c38, c39, c40, c41, c42, c43, c44, c45, c46, c47, c48, c49, c50, c51, c52, c53, c54, c55, c56, c57, c58, c59, c60, c61, c62, c63, c64 };

        cheepDbContext.Authors.AddRange(authors);
        cheepDbContext.Cheeps.AddRange(cheeps);
        cheepDbContext.SaveChanges();
    }
}


