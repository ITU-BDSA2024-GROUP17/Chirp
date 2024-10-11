using Web.Entities;

namespace Web.Interfaces;

public interface ICheepService
{
    public interface ICheeps
    {
        public List<Cheep> GetCheeps();

        public Task<IEnumerable<Cheep>> SearchCheeps(string searchQuery, int page);

        public Task<IEnumerable<Cheep>> GetAllCheeps(int page);

        public void StoreCheep(Cheep cheep);
    }
    public interface IAuthor
    {
        public List<Author> GetAuthors();
        public Task<Author> GetAuthorByField(string author, Func<Author, string> field);
        public Task<IEnumerable<Author>> SearchAuthors(string searchQuery, int page);
        public Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int page);
        public void createAuthor(Author author);
    }
}


