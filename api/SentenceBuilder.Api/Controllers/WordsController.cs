using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentenceBuilder.Api.Data;
using SentenceBuilder.Api.Dtos;

namespace SentenceBuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController(AppDbContext db) : ControllerBase
{
    /// <summary>All word types, each with the words that can be selected for it.</summary>
    [HttpGet]
    public async Task<ActionResult<List<WordTypeDto>>> Get(CancellationToken ct)
    {
        var result = await db.WordTypes
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .Select(t => new WordTypeDto(
                t.Id,
                t.Name,
                t.Words.OrderBy(w => w.Text).Select(w => new WordDto(w.Id, w.Text)).ToList()))
            .ToListAsync(ct);

        return result;
    }
}
