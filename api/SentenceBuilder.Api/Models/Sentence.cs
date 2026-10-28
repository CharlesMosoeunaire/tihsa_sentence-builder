namespace SentenceBuilder.Api.Models;

public class Sentence
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<SentenceWord> SentenceWords { get; set; } = new List<SentenceWord>();
}
