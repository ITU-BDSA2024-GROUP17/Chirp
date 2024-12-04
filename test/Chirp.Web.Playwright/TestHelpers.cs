using Microsoft.Playwright;

namespace Chirp.Web.Playwright;


[TestFixture]
public class TestHelpers(PageTest pageTest)
{
    readonly IPage Page = pageTest.Page;

    public async Task<int> HelperCreateAccount()
    {
        int userId = new Random().Next(1000, 1000000);

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Register" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/Identity/Account/Register");

        await Page.GetByPlaceholder("John Doe").FillAsync("Playwright" + userId);
        await Page.GetByPlaceholder("name@example.com").FillAsync("play" + userId + "@wright.dev");
        await Page.GetByRole(AriaRole.Textbox, new() { NameString = "Password" }).FillAsync("aB%" + userId);
        await Page.GetByLabel("Confirm Password").FillAsync("aB%" + userId);

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Create account" }).ClickAsync();
        await Page.WaitForURLAsync("http://localhost:5163/?page=1");

        await pageTest.Expect(Page.GetByRole(AriaRole.Button, new() { NameString = "PlayWright" + userId })).ToBeVisibleAsync();

        return userId;
    }

}
