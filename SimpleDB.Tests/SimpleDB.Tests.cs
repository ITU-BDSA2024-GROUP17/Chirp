using Xunit;
using Xunit.Sdk;

namespace SimpleDB.Tests;

public class CSVDatabaseTests
{

    [Fact]
    public void TestCreateDB()
    {

    }

    [Fact]
    public void TestRead()
    {
        // Arrange
        var database = new CSVDatabase<int>("../test.csv", true);

        // Act
        var records = database.Read();

        // Assert
        Assert.Empty(records);
    }
    [Fact]
    public void TestDelete()
    {
        // Arrange
        var database = new CSVDatabase<int>("../test.csv", true);

        // Act
        database.Delete();

        // Assert
        Assert.False(File.Exists("../test.csv"));
    }

    // TODO: Fix this test - does not store the value in the csv file
    // [Theory]
    // [InlineData(-1)]
    // [InlineData(0)]
    // [InlineData(1)]
    // public void TestStore(int val)
    // {

    //     CSVDatabase<int> database = new CSVDatabase<int>("../test.csv", true);
    //     // Act
    //     database.Store(val);
    //     var records = database.Read();

    //     // Assert
    //     Assert.Single(records);
    //     Assert.Equal(val, records.First());
    // }
}
