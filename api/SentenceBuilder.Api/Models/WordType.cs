namespace SentenceBuilder.Api.Models;

public class WordType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Word> Words { get; set; } = new List<Word>();
}
