using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder.ToTable("Moto");

            builder.HasKey(m => m.Id);
            builder.Property(f => f.Id)
               .ValueGeneratedOnAdd();

            builder.Property(m => m.Placa)
                .IsRequired()
                .HasMaxLength(7);

            builder.HasIndex(m => m.Placa)
    .IsUnique();

            builder.Property(m => m.Modelo)
                .HasMaxLength(50);

            builder.Property(m => m.Ano)
                .IsRequired();

            builder.Property(m => m.TipoCombustivel)
                .HasMaxLength(50);

            builder.Property(m => m.IdFilial)
                .IsRequired();

            builder.HasOne(f => f.Filial)
                .WithMany()
                .HasForeignKey(f => f.IdFilial)
                .OnDelete(DeleteBehavior.Cascade);
            }
    }
}
