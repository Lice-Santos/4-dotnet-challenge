using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class FilialMapping : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder.ToTable("Filial");

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id)
                   .ValueGeneratedOnAdd();



            builder.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.IdEndereco)
                .IsRequired();

            builder.HasOne(f => f.Endereco)
                .WithMany()
                .HasForeignKey(f => f.IdEndereco)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
