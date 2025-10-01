using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class FuncionarioMapping : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            // Tabela sem aspas, em maiúsculas (Oracle não case-sensitive sem aspas)
            builder.ToTable("FUNCIONARIO");

            // Chave primária
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id)
                   .HasColumnName("ID")
                   .ValueGeneratedOnAdd(); // Garante identidade no Oracle

            // Colunas mapeadas sem aspas
            builder.Property(f => f.Nome)
                   .HasColumnName("NOME")
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(f => f.Cargo)
                   .HasColumnName("CARGO")
                   .HasMaxLength(50);

            builder.Property(f => f.Email)
                   .HasColumnName("EMAIL")
                   .IsRequired()
                   .HasMaxLength(80);

            builder.Property(f => f.Senha)
                   .HasColumnName("SENHA")
                   .IsRequired()
                   .HasMaxLength(30);
        }
    }
}
