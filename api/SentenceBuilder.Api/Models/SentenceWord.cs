namespace SentenceBuilder.Api.Models;

public class SentenceWord
{
    public int Id { get; set; }
    public int SentenceId { get; set; }
    public Sentence? Sentence { get; set; }
    public int WordId { get; set; }
    public Word? Word { get; set; }
    public int Position { get; set; }
}
