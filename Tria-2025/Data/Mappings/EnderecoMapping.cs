using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Endereco");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.Cidade)
                .HasMaxLength(50);

            builder.Property(e => e.Estado)
                .HasMaxLength(50);

            builder.Property(e => e.Numero)
                .HasMaxLength(10);

            builder.Property(e => e.Complemento)
                .HasMaxLength(100);

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(8);
        }
    }
}
