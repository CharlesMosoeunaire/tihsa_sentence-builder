namespace SentenceBuilder.Api.Models;

public class Word
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int WordTypeId { get; set; }
    public WordType? WordType { get; set; }
}
