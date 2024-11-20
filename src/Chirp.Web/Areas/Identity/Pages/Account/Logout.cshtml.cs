// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel(SignInManager<Author> signInManager, ILogger<LogoutModel> logger) : PageModel
    {
        private readonly SignInManager<Author> _signInManager = signInManager;
        private readonly ILogger<LogoutModel> _logger = logger;

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
