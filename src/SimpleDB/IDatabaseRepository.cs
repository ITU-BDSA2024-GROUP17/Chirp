namespace SimpleDB;

interface IDatabaseRepository<T>
{
    public Task<List<T>> Read(int? limit = null);
    public void Store(T record);
}
