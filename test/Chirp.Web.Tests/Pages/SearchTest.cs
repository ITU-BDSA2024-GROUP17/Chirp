
namespace Chirp.Web.Tests.Integration
{
    public class SearchIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public SearchIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            CustomWebApplicationFactory<Program>.TestSeedDatabase(factory);
            _factory = factory;
        }


        [Theory]
        [InlineData("John Doe")]
        [InlineData("John Smith")]
        [InlineData("Jane Doe")]
        [InlineData("Jane Smith")]
        public async Task OnGet_SearchByAuthor_ReturnsAuthors(string user)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/search?SearchQuery=J&page=1");
            var page = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains(user, page);
            Assert.DoesNotContain("Helge", page);

            // Act for "@" endpoint
            var response2 = await client.GetAsync("/search?SearchQuery=@J&page=1");
            var page2 = await response2.Content.ReadAsStringAsync();
            Assert.Contains(user, page2);
            Assert.DoesNotContain("Helge", page2);
        }

        public static readonly TheoryData<int, string> CaseCheepSearch =
        new()
        {
            { 0, "I love my" },
            { 1, "I love my wife" },
            { 2, "I love my wife Jane Doe" },
        };
        [Theory, MemberData(nameof(CaseCheepSearch))]
        public async Task OnGet_SearchByCheep_ReturnsCheeps(int index, string cheep)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/search?SearchQuery={cheep}&page=1");
            var page = await response.Content.ReadAsStringAsync();

            // Assert
            switch (index)
            {
                case 0:
                case 1:
                    Assert.Contains("Jane Doe", page);
                    Assert.Contains("Jane Smith", page);
                    break;
                case 2:
                    Assert.Contains("I love my wife Jane Doe", page);
                    Assert.DoesNotContain("I love my wife John Doe", page);
                    break;

            }
        }

        public static readonly TheoryData<int, string> CaseCheepAndAuthorSearch =
        new()
        {
            // { 0, "@Jane D" },
            // { 1, "#I love my wife Jane" },
            { 2, "I love my wife Jane Doe" },
            { 3, "I love my wife John Smith" },
        };
        [Theory, MemberData(nameof(CaseCheepAndAuthorSearch))]
        public async Task OnGet_SearchByAuthorAndCheep_ReturnsResults(int index, string query)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/search?SearchQuery={query}&page=1");
            var page = await response.Content.ReadAsStringAsync();

            switch (index)
            {
                case 0:
                    Assert.Contains("John Doe", page);
                    Assert.DoesNotContain("Jane Smith", page);
                    break;
                case 1:
                    Assert.Contains("I love my wife Jane Doe", page);
                    Assert.Contains("I love my wife Jane Smith", page);
                    Assert.DoesNotContain("I love my husband John Doe", page);
                    break;
                case 2:
                    Assert.Contains("I love my wife Jane Doe", page);
                    Assert.DoesNotContain("I love my wife John Doe", page);
                    Assert.DoesNotContain("I love my wife John Smith", page);
                    break;
                case 3:
                    Assert.Contains("I love my wife John Smith", page);
                    Assert.DoesNotContain("I love my wife Jane Doe", page);
                    Assert.DoesNotContain("I love my wife John Doe", page);
                    break;
            }
        }
    }
}
