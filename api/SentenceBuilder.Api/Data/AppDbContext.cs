using Microsoft.EntityFrameworkCore;
using SentenceBuilder.Api.Models;

namespace SentenceBuilder.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WordType> WordTypes => Set<WordType>();
    public DbSet<Word> Words => Set<Word>();
    public DbSet<Sentence> Sentences => Set<Sentence>();
    public DbSet<SentenceWord> SentenceWords => Set<SentenceWord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WordType>(e =>
        {
            e.Property(x => x.Name).IsRequired().HasMaxLength(50);
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Word>(e =>
        {
            e.Property(x => x.Text).IsRequired().HasMaxLength(100);
            e.HasIndex(x => new { x.WordTypeId, x.Text }).IsUnique();
            e.HasOne(x => x.WordType)
             .WithMany(t => t.Words)
             .HasForeignKey(x => x.WordTypeId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Sentence>(e =>
        {
            e.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<SentenceWord>(e =>
        {
            e.HasOne(x => x.Sentence)
             .WithMany(s => s.SentenceWords)
             .HasForeignKey(x => x.SentenceId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Word)
             .WithMany()
             .HasForeignKey(x => x.WordId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.SentenceId, x.Position });
        });

        var (types, words) = SeedData.Build();
        modelBuilder.Entity<WordType>().HasData(types);
        modelBuilder.Entity<Word>().HasData(words);
    }
}
