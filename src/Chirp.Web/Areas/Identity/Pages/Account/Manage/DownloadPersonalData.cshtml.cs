// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text.Json;
using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel(
    UserManager<Author> userManager,
    ILogger<DownloadPersonalDataModel> logger) : PageModel
{
    private readonly UserManager<Author> _userManager = userManager;
    private readonly ILogger<DownloadPersonalDataModel> _logger = logger;

    public IActionResult OnGet()
    {
        return NotFound();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

        // Explicitly load collections to be included later
        var context = HttpContext.RequestServices.GetService<CheepDbContext>();
        if (context != null)
        {
            await context.Entry(user).Collection(u => u.Cheeps)
                .Query()
                .Include(c => c.Revisions)
                .Include(c => c.Likes)
                .LoadAsync();

            await context.Entry(user).Collection(u => u.LikedCheeps)
                .Query()
                .Include(c => c.Author)
                .Include(c => c.Revisions)
                .Include(c => c.Likes)
                .LoadAsync();

            await context.Entry(user).Collection(u => u.Following).LoadAsync();
            await context.Entry(user).Collection(u => u.Followers).LoadAsync();
        }

        // Only include personal data for download
        var personalData = new Dictionary<string, object>();
        var personalDataProps = typeof(Author).GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        foreach (var p in personalDataProps)
        {
            var value = p.GetValue(user);

            // Serilize the data based on the type.
            if (p.Name == nameof(user.Cheeps))
            {
                personalData.Add(p.Name, user.Cheeps.Select(c => new
                {
                    Revisions = c.Revisions.Select(r => new
                    {
                        r.Message,
                        r.TimeStamp
                    }),
                    Likes = c.Likes.Select(l => l.UserName)
                }));
            }
            else if (p.Name == nameof(user.LikedCheeps))
            {
                personalData.Add(p.Name, user.LikedCheeps.Select(c => new
                {
                    Author = c.Author.UserName,
                    Revisions = c.Revisions.Select(r => new
                    {
                        r.Message,
                        r.TimeStamp
                    }),
                    Likes = c.Likes.Select(l => l.UserName)
                }));
            }
            else if (value is IEnumerable<object> collection)
            {
                personalData.Add(p.Name, collection.Select(item => item.ToString()).ToList());
            }
            else
            {
                personalData.Add(p.Name, value?.ToString() ?? "null");
            }
        }

        var logins = await _userManager.GetLoginsAsync(user);
        foreach (var l in logins)
        {
            personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

        Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
        return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
    }
}
