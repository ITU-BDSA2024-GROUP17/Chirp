namespace SimpleDB.Tests;

public record Test(int Id);

public class CSVDatabaseTests
{
    readonly CSVDatabase<Test> database;

    public CSVDatabaseTests()
    {
        database = CSVDatabase<Test>.Instance;

        database.Store(new Test(-1));
        database.Store(new Test(0));
        database.Store(new Test(1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void TestStoreRead(int val)
    {
        // Act
        var records = database.Read();

        // Assert
        Assert.Contains(new Test(val), records);

        // Cleanup
        database.Clear();
    }

    [Fact]
    public void TestRead()
    {
        // Act
        var records = database.Read();

        // Assert
        Assert.NotEmpty(records);
    }
}
