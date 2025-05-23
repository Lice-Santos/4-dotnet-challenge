using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class FuncionarioMapping : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            //Mapeando restrições e linhas da tabela Funcionario
            builder.ToTable("Funcionario");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(f => f.Cargo)
                .HasMaxLength(50);

            builder.Property(f => f.Email)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(f => f.Senha)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
