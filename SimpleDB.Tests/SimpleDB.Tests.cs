using Xunit;
using Xunit.Sdk;

namespace SimpleDB.Tests;



public record test(int id)
{
}

public class CSVDatabaseTests
{
    // // TODO: This fails, to be continued
    // [Theory]
    // [InlineData(-1)]
    // [InlineData(0)]
    // [InlineData(1)]
    // public void TestStore(int val)
    // {
    //     CSVDatabase<test> database = CSVDatabase<test>.Instance;
    //     // Act
    //     database.Store(new test(val));
    //     var records = database.Read();

    //     // Assert
    //     Assert.Single(records);
    //     Assert.Equal(new test(val), records.First());
    //     database.Clear();
    // }


    // [Fact]
    // public void TestClear()
    // {

    // }

    // [Fact]
    // public void TestRead()
    // {
    //     // Arrange
    //     CSVDatabase<int> database = CSVDatabase<int>.Instance;

    //     database.Clear();
    //     // Act
    //     var records = database.Read();

    //     // Assert
    //     Assert.Empty(records);
    // }
}
