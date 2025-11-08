using Microsoft.EntityFrameworkCore;
using SmartApi.Web.Models;
using Tria_2025.Data.Mappings;
using Tria_2025.Models;

namespace Tria_2025.Connection
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<MotoSetor> Moto_Setores { get; set; }
        public DbSet<Setor> Setores { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoMapping());
            modelBuilder.ApplyConfiguration(new FuncionarioMapping());
            modelBuilder.ApplyConfiguration(new EnderecoMapping());
            modelBuilder.ApplyConfiguration(new FilialMapping());
            modelBuilder.ApplyConfiguration(new MotoSetorMapping());
            modelBuilder.ApplyConfiguration(new SetorMapping());
            modelBuilder.ApplyConfiguration(new ReviewMapping());


        }
    }
}
