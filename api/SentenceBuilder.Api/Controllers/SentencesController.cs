using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentenceBuilder.Api.Data;
using SentenceBuilder.Api.Dtos;
using SentenceBuilder.Api.Models;

namespace SentenceBuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SentencesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SentenceDto>>> GetAll(CancellationToken ct)
    {
        var sentences = await ReadQuery()
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(ct);

        return sentences.Select(ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SentenceDto>> GetById(int id, CancellationToken ct)
    {
        var sentence = await ReadQuery().FirstOrDefaultAsync(s => s.Id == id, ct);
        return sentence is null ? NotFound() : ToDto(sentence);
    }

    [HttpPost]
    public async Task<ActionResult<SentenceDto>> Create(SentenceRequest request, CancellationToken ct)
    {
        var problem = await ValidateAsync(request, ct);
        if (problem is not null) return problem;

        var sentence = new Sentence { CreatedAt = DateTime.UtcNow };
        AddWords(sentence, request.WordIds);

        db.Sentences.Add(sentence);
        await db.SaveChangesAsync(ct);

        var created = await ReadQuery().FirstAsync(s => s.Id == sentence.Id, ct);
        return CreatedAtAction(nameof(GetById), new { id = sentence.Id }, ToDto(created));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SentenceDto>> Update(int id, SentenceRequest request, CancellationToken ct)
    {
        var sentence = await db.Sentences
            .Include(s => s.SentenceWords)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (sentence is null) return NotFound();

        var problem = await ValidateAsync(request, ct);
        if (problem is not null) return problem;

        db.SentenceWords.RemoveRange(sentence.SentenceWords);
        sentence.SentenceWords.Clear();
        AddWords(sentence, request.WordIds);
        sentence.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        var updated = await ReadQuery().FirstAsync(s => s.Id == id, ct);
        return ToDto(updated);
    }

    // ---- helpers ----

    private IQueryable<Sentence> ReadQuery() =>
        db.Sentences
            .AsNoTracking()
            .Include(s => s.SentenceWords.OrderBy(sw => sw.Position))
                .ThenInclude(sw => sw.Word)
                    .ThenInclude(w => w!.WordType);

    private async Task<ActionResult?> ValidateAsync(SentenceRequest request, CancellationToken ct)
    {
        if (request.WordIds is null || request.WordIds.Count == 0)
            return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["wordIds"] = ["A sentence must contain at least one word."]
            }));

        var requested = request.WordIds.Distinct().ToList();
        var existing = await db.Words
            .Where(w => requested.Contains(w.Id))
            .Select(w => w.Id)
            .ToListAsync(ct);

        var unknown = requested.Except(existing).ToList();
        if (unknown.Count > 0)
            return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["wordIds"] = [$"Unknown word id(s): {string.Join(", ", unknown)}."]
            }));

        return null;
    }

    private static void AddWords(Sentence sentence, List<int> wordIds)
    {
        for (var i = 0; i < wordIds.Count; i++)
            sentence.SentenceWords.Add(new SentenceWord { WordId = wordIds[i], Position = i });
    }

    private static SentenceDto ToDto(Sentence s)
    {
        var words = s.SentenceWords
            .OrderBy(sw => sw.Position)
            .Select(sw => new SentenceWordDto(
                sw.WordId,
                sw.Word!.Text,
                sw.Word.WordTypeId,
                sw.Word.WordType!.Name,
                sw.Position))
            .ToList();

        return new SentenceDto(
            s.Id,
            string.Join(' ', words.Select(w => w.Text)),
            s.CreatedAt,
            s.UpdatedAt,
            words);
    }
}
