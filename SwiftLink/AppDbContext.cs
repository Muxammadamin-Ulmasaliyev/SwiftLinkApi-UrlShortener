using Microsoft.EntityFrameworkCore;
using SwiftLink.Entities;
using SwiftLink.Services;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SwiftLink
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ShortenedURL> ShortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortenedURL>(builder =>
            {
                builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);
                builder.HasIndex(s => s.Code).IsUnique();
            });
        }
    }
}
