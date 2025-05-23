using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tria_2025.Models;

namespace Tria_2025.Data.Mappings
{
    public class MotoSetorMapping : IEntityTypeConfiguration<MotoSetor>
    {
        public void Configure(EntityTypeBuilder<MotoSetor> builder)
        {
            //Mapeando restrições e linhas da tabela MotoSetor
            builder.ToTable("Moto_Setor");

            builder.HasKey(f => f.Id);

            builder.Property(ms => ms.Data)
                .IsRequired();

            builder.Property(ms => ms.Fonte)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ms => ms.IdMoto)
                .IsRequired();

            builder.Property(ms => ms.IdSetor)
            .IsRequired();

            //Declarando os relacionamentos e as chaves estrangeiras da tabela

            builder.HasOne(ms => ms.Moto)
                .WithMany() 
                .HasForeignKey(ms => ms.IdMoto)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ms => ms.Setor)
                .WithMany() 
                .HasForeignKey(ms => ms.IdSetor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
