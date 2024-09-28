﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDB.Records;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    // [HttpGet]
    // [Route("/{page}")]
    public ActionResult OnGet([FromQuery] int page = 1)
    {
        Cheeps = _service.GetCheeps().Result;
        return Page();
    }
}
