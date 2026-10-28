using SentenceBuilder.Api.Models;

namespace SentenceBuilder.Api.Data;

/// <summary>
/// Reference data that is written to the database by the EF migration.
/// The API never serves words from code - only from the database.
/// </summary>
public static class SeedData
{
    public static (List<WordType> Types, List<Word> Words) Build()
    {
        (string Type, string[] Words)[] catalog =
        [
            ("Noun",        ["cat", "dog", "teacher", "house", "river", "book", "city", "music", "doctor", "garden"]),
            ("Verb",        ["runs", "eats", "jumps", "writes", "builds", "reads", "sings", "plays", "opens", "loves"]),
            ("Adjective",   ["happy", "quick", "large", "quiet", "bright", "old", "clever", "brave", "cold", "red"]),
            ("Adverb",      ["quickly", "slowly", "happily", "quietly", "often", "always", "never", "carefully", "loudly", "soon"]),
            ("Pronoun",     ["I", "you", "he", "she", "it", "we", "they", "him", "her", "them"]),
            ("Preposition", ["in", "on", "under", "over", "with", "through", "between", "near", "behind", "across"]),
            ("Conjunction", ["and", "but", "or", "because", "although", "while", "so", "yet", "if", "since"]),
            ("Determiner",  ["the", "a", "an", "this", "that", "these", "those", "my", "some", "every"]),
            ("Exclamation", ["Wow", "Oops", "Hooray", "Ouch", "Hey", "Yikes", "Bravo", "Alas", "Phew", "Congratulations"]),
        ];

        var types = new List<WordType>();
        var words = new List<Word>();
        var wordId = 1;

        for (var i = 0; i < catalog.Length; i++)
        {
            var typeId = i + 1;
            types.Add(new WordType { Id = typeId, Name = catalog[i].Type });

            foreach (var text in catalog[i].Words)
                words.Add(new Word { Id = wordId++, Text = text, WordTypeId = typeId });
        }

        return (types, words);
    }
}
