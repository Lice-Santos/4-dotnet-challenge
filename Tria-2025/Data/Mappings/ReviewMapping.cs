using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class ReviewMapping : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Text)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(r => r.PredictedSentiment)
                .HasMaxLength(20);

            builder.Property(r => r.SentimentScore);
        }
    }
}
