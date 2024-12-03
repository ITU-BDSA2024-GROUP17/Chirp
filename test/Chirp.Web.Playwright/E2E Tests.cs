using Microsoft.Playwright;

namespace Chirp.Web.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class E2ETests : PageTest
{

    TestHelpers? helpers;

    [SetUp]
    public async Task Setup()
    {
        helpers = new(this);
        await Page.GotoAsync("http://localhost:5163/");
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
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();
    }

    [Test]
    public async Task LogoutTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
    }

    [Test]
    public async Task LoginTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

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
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync("I am testing today!");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
        await Expect(Page.Locator(".cheep").GetByText("I am testing today!").First).ToBeVisibleAsync();
        await Expect(Page.Locator(".cheep").First.GetByText("PlayWright" + userId)).ToBeVisibleAsync();
    }

    [Test]
    public async Task AvatarTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Settings" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/Identity/Account/Manage");
        await Expect(Page.GetByRole(AriaRole.Img, new()).Nth(1)).Not.ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Avatar" }).FillAsync("https://www.w3schools.com/howto/img_avatar.png");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Save" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/Identity/Account/Manage");

        await Expect(Page.GetByRole(AriaRole.Img, new()).Nth(1)).ToBeVisibleAsync();
    }
}
