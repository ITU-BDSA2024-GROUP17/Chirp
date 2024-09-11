using System.Diagnostics;
using Xunit;

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
        // [Fact]
        // public void TestReadCommand()
        // {
        //     string[] commands = ["cheep Hello World!", "read"];
        //     string[] expected = ["", $"{Environment.MachineName} @ {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}: Hello World!"];
        //     // Act
        //     for (int i = 0; i < 1; i++)
        //     {
        //         string actual = RunProgramWithArguments(commands[i]);
        //         // Assert
        //         Assert.Equal(expected[i], actual);
        //     }

        // }

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
                throw new Exception($"Process exited with code {process.ExitCode}: {error}");
            }

            return output;
        }
    }
}
