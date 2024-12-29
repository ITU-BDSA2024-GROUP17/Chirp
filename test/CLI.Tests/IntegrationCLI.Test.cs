using System.Diagnostics;
using System.Text.RegularExpressions;
using CLI.Records;
using SimpleDB;

namespace CLI.Tests;

public class IntegrationCLI_Test
{
    [Fact]
    public void TestVersionCommand()
    {
        // Arrange
        string expected = "chirp 0.1";
        string arguments = "--version";
        // Act
        string actual = RunProgramWithArguments(arguments);

        // Assert
        Assert.Contains(expected, actual);
    }

    [Fact]
    public void GetCheepsTest()
    {
        // Arrange
        var db = CSVDatabase<Cheep>.Instance;
        // Act
        var cheeps = db.Read();
        // Assert

        foreach (var cheep in cheeps)
        {
            Assert.NotNull(cheep);

            // Assert if Cheep is of right type
            Assert.IsType<Cheep>(cheep);

            Assert.IsType<string>(cheep.Author);
            Assert.IsType<string>(cheep.Message);
            Assert.IsType<long>(cheep.Timestamp);
        }
        db.Clear();
    }

    [Theory]
    [InlineData("cheep")]
    [InlineData("halp")]
    [InlineData("help")]
    [InlineData("")]
    public void TestErrorCommand(string command)
    {
        // Arrange
        const string help = @"chirp.
            Usage:
            read               Read all cheeps
            cheep <message>    Cheep a message
            Options:
                -h --help     Show this screen.
                --version     Show version.
            ";

        // Act
        string actual = RunProgramWithArguments(command);

        // Assert
        // Replace "\r\n" with " " to ignore line endings
        Assert.Equal(help.Replace("\r\n", "").Replace("\n", "").Replace(" ", ""), actual.Replace("\r\n", "").Replace("\n", "").Replace(" ", ""),
        ignoreWhiteSpaceDifferences: true,
        ignoreLineEndingDifferences: true,
        ignoreAllWhiteSpace: true);
    }

    private static string RunProgramWithArguments(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run -- {arguments}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = "../../../../../src/CLI/"
        };
        var process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            return error;
        }

        return output;
    }
}
