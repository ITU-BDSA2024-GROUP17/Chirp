using System.Diagnostics;
using System.Text.RegularExpressions;
using CLI.Records;
using CsvHelper.Configuration.Attributes;
using SimpleDB;


namespace CLI.Tests
{
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
        public void getCheepsTest()
        {
            // Arrange
            var db = new CSVDatabase<Cheep>();
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
            Assert.Equal(help, actual.Replace("\r\n", " "),
            ignoreWhiteSpaceDifferences: true,
            ignoreLineEndingDifferences: true,
            ignoreAllWhiteSpace: true);
        }

        [Fact]
        public void TestReadCommand()
        {
            string command = "read";

            var actual = RunProgramWithArguments(command);

            // Regex for dates
            string datePattern = @"(0[1-9]|[12][0-9]|3[01])(\/|-)(0[1-9]|1[1,2])(\/|-)(19|20)\d{2}";
            Match match = Regex.Match(actual, datePattern);

            // Assert
            Assert.True(match.Success); // Check if date is present
            Assert.Contains(Environment.MachineName, actual); // Check if machine name is present
        }

        private string RunProgramWithArguments(string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run -- {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = "../../../../CLI"
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
}
