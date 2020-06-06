using ConstructionExchangeQuotes.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionExchangeQuotes.Server
{
    public class QuotesDbContext : DbContext
    {
        public QuotesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Element> Elements { get; set; }
        public DbSet<ElementCategory> ElementCategories { get; set; }
        public DbSet<ElementField> ElementFields { get; set; }
        public DbSet<ElementType> ElementTypes { get; set; }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteElement> QuoteElements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Element>()
                .HasOne(x => x.ElementCategory)
                .WithMany(x => x.Elements)
                .HasForeignKey(x => x.ElementCategoryId);

            modelBuilder.Entity<Element>()
                .HasOne(x => x.ElementType)
                .WithMany(x => x.Elements)
                .HasForeignKey(x => x.ElementTypeId);

            modelBuilder.Entity<ElementField>()
                .HasOne(x => x.Element)
                .WithMany(x => x.ElementFields)
                .HasForeignKey(x => x.ElementId);

            modelBuilder.Entity<QuoteElement>()
                .HasKey(x => new
                {
                    x.QuoteId,
                    x.ElementId
                });

            modelBuilder.Entity<QuoteElement>()
                .HasOne(x => x.Element)
                .WithMany(x => x.QuoteElements)
                .HasForeignKey(x => x.ElementId);

            modelBuilder.Entity<QuoteElement>()
                .HasOne(x => x.Quote)
                .WithMany(x => x.QuoteElements)
                .HasForeignKey(x => x.QuoteId);
        }
    }
}
