using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity;

public class CheeperSignInManager(
    UserManager<Author> userManager,
    Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor,
    IUserClaimsPrincipalFactory<Author> claimsFactory,
    Microsoft.Extensions.Options.IOptions<IdentityOptions> optionsAccessor,
    Microsoft.Extensions.Logging.ILogger<SignInManager<Author>> logger,
    Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes,
    IUserConfirmation<Author> confirmation) : SignInManager<Author>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
{
    public override async Task<SignInResult> PasswordSignInAsync(
        string email,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        var user = await UserManager.FindByEmailAsync(email);

        if (user == null) return SignInResult.Failed;
        else return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }
}
