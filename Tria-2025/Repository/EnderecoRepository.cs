using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;
using Tria_2025.Repository;

namespace Tria_2025.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly AppDbContext _context;

        public EnderecoRepository(AppDbContext context)
        {
            // O AppDbContext é injetado, exatamente como no seu Controller original
            _context = context;
        }

        // --- MÉTODOS CRUD BÁSICOS ---

        public async Task<Endereco> AddAsync(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
            await SaveChangesAsync();
            return endereco;
        }

        public async Task UpdateAsync(Endereco endereco)
        {
            // Attach e State = Modified são usados para garantir que o EF rastreie as alterações
            _context.Enderecos.Attach(endereco);
            _context.Entry(endereco).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Endereco endereco)
        {
            _context.Enderecos.Remove(endereco);
            await SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Endereco> GetByIdAsync(int id)
        {
            // Usa FindAsync que é otimizado para buscar pela chave primária
            return await _context.Enderecos.FindAsync(id);
        }

        public async Task<IEnumerable<Endereco>> GetAllAsync()
        {
            return await _context.Enderecos.ToListAsync();
        }

        // --- MÉTODOS DE BUSCA ESPECÍFICA (Transf. do seu Controller) ---

        public async Task<Endereco> GetByCepAsync(string cep)
        {
            // Retorna o primeiro Endereço que corresponda ao CEP
            return await _context.Enderecos.FirstOrDefaultAsync(c => c.Cep == cep);
        }

        public async Task<IEnumerable<Endereco>> GetByLogradouroAsync(string logradouro)
        {
            // Retorna a lista de Endereços que contenham o Logradouro, ignorando case
            return await _context.Enderecos
                                 .Where(c => c.Logradouro.ToLower().Contains(logradouro.ToLower()))
                                 .ToListAsync();
        }

        // --- MÉTODOS AUXILIARES ---

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Enderecos.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Retorna true se houver mais de 0 alterações salvas
            return await _context.SaveChangesAsync() > 0;
        }
    }
}