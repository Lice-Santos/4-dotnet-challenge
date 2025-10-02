using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class SetorMapping : IEntityTypeConfiguration<Setor>
    {
        public void Configure(EntityTypeBuilder<Setor> builder)
        {
            builder.ToTable("Setor");

            builder.HasKey(s => s.Id);

            builder.Property(f => f.Id)
       .ValueGeneratedOnAdd();

            builder.Property(s => s.Nome)
                .IsRequired();
        }
    }
}
