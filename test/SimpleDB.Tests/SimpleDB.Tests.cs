namespace SimpleDB.Tests;

public record Test(int Id);

public class CSVDatabaseTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void TestStore(int val)
    {
        var database = CSVDatabase<Test>.Instance;

        // Act
        database.Store(new Test(val));
        var records = database.Read();

        // Assert
        Assert.Single(records);
        Assert.Equal(new Test(val), records.First());

        // Cleanup
        database.Clear();
    }

    [Fact]
    public void TestRead()
    {
        // Arrange
        CSVDatabase<int> database = CSVDatabase<int>.Instance;

        database.Clear();
        // Act
        var records = database.Read();

        // Assert
        Assert.Empty(records);
    }
}
