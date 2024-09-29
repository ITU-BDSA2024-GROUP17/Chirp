using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;
using Xunit.Sdk;
using System.Data;
using SimpleDB.Records;
namespace SimpleDB.Tests;


public class CSVDatabaseTests
{
    [Fact]
    public void TestConnection()
    {
        // Act
        var connection = CSVDatabase.connection != null && CSVDatabase.connection.State == ConnectionState.Open;

        // Assert
        Assert.True(connection);
    }


}
