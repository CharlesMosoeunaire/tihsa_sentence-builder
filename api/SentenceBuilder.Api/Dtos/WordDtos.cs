namespace SentenceBuilder.Api.Dtos;

public record WordDto(int Id, string Text);

public record WordTypeDto(int Id, string Name, List<WordDto> Words);
