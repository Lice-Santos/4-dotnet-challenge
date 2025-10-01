using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public class SetorRepository : ISetorRepository
    {
        private readonly AppDbContext _context;

        public SetorRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS CRUD BÁSICOS ---

        public async Task<Setor> AddAsync(Setor setor)
        {
            _context.Setores.Add(setor);
            await SaveChangesAsync();
            return setor;
        }

        public async Task UpdateAsync(Setor setor)
        {
            _context.Setores.Attach(setor);
            _context.Entry(setor).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Setor setor)
        {
            _context.Setores.Remove(setor);
            await SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Setor> GetByIdAsync(int id)
        {
            return await _context.Setores.FindAsync(id);
        }

        public async Task<IEnumerable<Setor>> GetAllAsync()
        {
            return await _context.Setores.ToListAsync();
        }

        // --- MÉTODO ESPECÍFICO PARA VALIDAÇÃO ---

        // Verifica se já existe um setor com o mesmo nome (ignorando case)
        public async Task<bool> NomeSetorExistsAsync(string nome)
        {
            return await _context.Setores
                                   .CountAsync(s => s.Nome == nome) > 0;
        }

        // --- MÉTODO AUXILIAR ---

        public async Task<bool> SaveChangesAsync()
        {
            // Salva as alterações e retorna true se houver sucesso
            return await _context.SaveChangesAsync() > 0;
        }
    }
}