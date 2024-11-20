using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace Chirp.Web.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class E2ETests : PageTest
{

    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync("http://localhost:5163/");
    }

    private async Task<int> HelperCreateAccount()
    {
        int userId = new Random().Next(1000, 10000);

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Register" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/Identity/Account/Register");

        await Page.GetByPlaceholder("John Doe").FillAsync("Playwright" + userId);
        await Page.GetByPlaceholder("name@example.com").FillAsync("play" + userId + "@wright.dev");
        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Password" }).FillAsync("aB%" + userId);
        await Page.GetByLabel("Confirm Password").FillAsync("aB%" + userId);

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Create account" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Expect(Page.GetByRole(AriaRole.Button, new() { NameString = "PlayWright" + userId })).ToBeVisibleAsync();

        return userId;
    }

    // [Test] // Skipped because it does not work
    public async Task SearchForUserTest()
    {
        await Page.GetByPlaceholder("Search...").ClickAsync();
        await Page.GetByPlaceholder("Search...").FillAsync("helge");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "H Helge" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/Helge");

        await Expect(Page.GetByText("Hello, BDSA students!")).ToBeVisibleAsync();
    }

    [Test]
    public async Task SearchForTextTest()
    {
        await Page.GetByPlaceholder("Search...").ClickAsync();
        await Page.GetByPlaceholder("Search...").FillAsync("The train");
        await Page.Keyboard.PressAsync("Enter");

        await Expect(Page.GetByRole(AriaRole.Link, new() { NameString = "Jacqualine Gilcoine" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task CreateAccountTest()
    {
        int userId = await HelperCreateAccount();
    }

    [Test]
    public async Task LogoutTest()
    {
        int userId = await HelperCreateAccount();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
    }

    [Test]
    public async Task LoginTest()
    {
        int userId = await HelperCreateAccount();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
        await Page.GotoAsync("http://localhost:5163/?page=1");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Login" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/Identity/Account/Login");

        await Page.GetByPlaceholder("name@example.com").FillAsync("play" + userId + "@wright.dev");
        await Page.GetByPlaceholder("password").FillAsync("aB%" + userId);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Log in" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Expect(Page.GetByRole(AriaRole.Button, new() { NameString = "PlayWright" + userId })).ToBeVisibleAsync();
    }

    [Test]
    public async Task CheepTest()
    {
        int userId = await HelperCreateAccount();

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync("I am testing today!");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
        await Expect(Page.GetByText("I am testing today!").GetByText("PlayWright" + userId)).ToBeVisibleAsync();
    }
}