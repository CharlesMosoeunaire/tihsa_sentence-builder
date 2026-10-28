namespace SentenceBuilder.Api.Dtos;

public record SentenceRequest(List<int> WordIds);

public record SentenceWordDto(int WordId, string Text, int WordTypeId, string WordTypeName, int Position);

public record SentenceDto(
    int Id,
    string Text,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<SentenceWordDto> Words);
