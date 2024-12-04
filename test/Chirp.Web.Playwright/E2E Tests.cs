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
        await Expect(Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId })).ToBeHiddenAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { NameString = "Login" })).ToBeVisibleAsync();

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

    [Test]
    public async Task EditCheepTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync("I am testing today!");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Page.Locator(".cheep-dropdown").First.Locator("button.btn").First.ClickAsync();
        await Page.Locator("button.dropdown-item").First.ClickAsync();
        await Page.Locator(".cheepeditbox").First.FillAsync("Second time testing today!");
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Expect(Page.Locator(".cheep").GetByText("Second time testing today!").Locator("visible=true").First).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Expect(Page.Locator(".cheep-dropdown")).ToHaveCountAsync(0);
    }

    [Test]
    public async Task CommentTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();
        var msg1 = "I am testing today!";
        var msg2 = "Second time testing today!";

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync(msg1);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Page.Locator(".cheep:has-text(\"Playwright" + userId + "\")").First.GetByRole(AriaRole.Button, new() { NameString = "Add comment" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "What's on your mind?" }).FillAsync(msg2);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Comment" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
        await Expect(Page.Locator(".cheep").GetByText(msg2).First).ToBeVisibleAsync();
    }

    [Test]
    public async Task CommentNotVisibleOnAuthorTimelineTest()
    {
        if (helpers == null) return;
        var cheepMessage = "Hello world!";
        var commentMessage = "Is this visible on the author timeline?";

        // Create cheep
        int userId1 = await helpers.HelperCreateAccount();
        await Page.GetByPlaceholder("Whats on your mind?").FillAsync(cheepMessage);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        // Logout
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId1 }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        // Create comment
        int userId2 = await helpers.HelperCreateAccount();
        await Page.Locator(".cheep:has-text(\"Playwright" + userId1 + "\")").First.GetByRole(AriaRole.Button, new() { NameString = "Add comment" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "What's on your mind?" }).FillAsync(commentMessage);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Comment" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        // Go to author page
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId2 }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Profile" }).ClickAsync();
        await Page.WaitForURLAsync($"http://localhost:5163/Playwright{userId2}?page=1");

        // Comment should not be visible
        var commentLocator = Page.Locator(".cheep").GetByText(commentMessage);
        await Expect(commentLocator).Not.ToBeVisibleAsync();
    }

    [Test]
    public async Task CommentVisibleOnAuthorCommentsTest()
    {
        if (helpers == null) return;
        var cheepMessage = "Hello world!";
        var commentMessage = "Is this visible on the author timeline?";

        // Create cheep
        int userId1 = await helpers.HelperCreateAccount();
        await Page.GetByPlaceholder("Whats on your mind?").FillAsync(cheepMessage);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        // Logout
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId1 }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        // Create comment
        int userId2 = await helpers.HelperCreateAccount();
        await Page.Locator(".cheep:has-text(\"Playwright" + userId1 + "\")").First.GetByRole(AriaRole.Button, new() { NameString = "Add comment" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "What's on your mind?" }).FillAsync(commentMessage);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Comment" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        // Go to author page
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId2 }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Profile" }).ClickAsync();
        await Page.WaitForURLAsync($"http://localhost:5163/Playwright{userId2}?page=1");

        // Go to author comments page
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex(".*Comments.*") }).ClickAsync();
        await Page.WaitForURLAsync($"http://localhost:5163/Playwright{userId2}/comments?page=1");

        // Comment should be visible
        var commentLocator = Page.Locator(".cheep").GetByText(commentMessage);
        await Expect(commentLocator).ToBeVisibleAsync();
    }

    [Test]
    public async Task DeleteTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();
        var msg1 = "I am testing today!";

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync(msg1);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        var cheep = Page.Locator(".cheep:has-text(\"Playwright" + userId + "\")").First;
        await cheep.GetByRole(AriaRole.Button, new() { NameString = "More options" }).ClickAsync();
        await cheep.GetByRole(AriaRole.Button, new() { NameString = "Delete" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");
        await Expect(Page.Locator(".cheep:has-text(\"Playwright" + userId + "\")").GetByText(msg1).First).ToBeHiddenAsync();
    }

    [Test]
    public async Task LikeCheepTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync("I am testing likes today!");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Expect(Page.Locator(".font-sec").First).ToHaveTextAsync("0");

        await Page.Locator(".like-btn").First.ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Expect(Page.Locator(".active").First).ToContainTextAsync("1");
    }

    [Test]
    public async Task RevisionsTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync("I am testing revisions today!");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Page.Locator(".cheep-dropdown").First.Locator("button.btn").First.ClickAsync();
        await Page.Locator("button.dropdown-item").First.ClickAsync();
        await Page.Locator(".cheepeditbox").First.FillAsync("Second time testing revisions today!");
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Page.GetByText("Edited").First.ClickAsync();

        await Expect(Page.Locator(".dropdown:has-text('Edited')").First.GetByText("I am testing revisions today!").First).ToBeVisibleAsync();
        await Expect(Page.Locator(".dropdown:has-text('Edited')").First.GetByText("Second time testing revisions today!").First).ToBeVisibleAsync();
    }

    [Test]
    public async Task AccountTimelineTest()
    {
        if (helpers == null) return;
        int userId = await helpers.HelperCreateAccount();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + userId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Profile" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/" + "Playwright" + userId + "?page=1");

        await Expect(Page.GetByText("" + userId).First).ToBeVisibleAsync();
    }

    [Test]
    public async Task UserTimelineTest()
    {
        if (helpers == null) return;
        int otherUserId = await helpers.HelperCreateAccount();

        await Page.GetByPlaceholder("Whats on your mind?").FillAsync("We are testing search and user timeline today!");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Cheep" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Playwright" + otherUserId }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Logout" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        if (helpers == null) return;
        await helpers.HelperCreateAccount();

        await Page.GetByPlaceholder("Search...").FillAsync("Playwright" + otherUserId);
        await Expect(Page.GetByText("Playwright" + otherUserId).First).ToBeVisibleAsync();
        await Page.Keyboard.PressAsync("ArrowDown");
        await Page.Keyboard.PressAsync("Enter");

        await Page.WaitForURLAsync("http://localhost:5163/" + "Playwright" + otherUserId + "?page=1");
        await Expect(Page.GetByText("We are testing search and user timeline today!").Locator("visible=true").First).ToBeVisibleAsync();

    }
}
