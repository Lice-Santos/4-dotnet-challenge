using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public class FilialRepository : IFilialRepository
    {
        private readonly AppDbContext _context;

        public FilialRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS CRUD BÁSICOS ---

        public async Task<Filial> AddAsync(Filial filial)
        {
            _context.Filiais.Add(filial); 
            await SaveChangesAsync();
            return filial;
        }




        public async Task UpdateAsync(Filial filial)
        {
            _context.Filiais.Attach(filial);
            _context.Entry(filial).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Filial filial)
        {
            _context.Filiais.Remove(filial);
            await SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Filial> GetByIdAsync(int id)
        {
            return await _context.Filiais.FindAsync(id);
        }

        public async Task<IEnumerable<Filial>> GetAllAsync()
        {
            return await _context.Filiais.ToListAsync();
        }

        // --- MÉTODO ESPECÍFICO PARA VALIDAÇÃO ---

        public async Task<bool> NomeFilialExistsAsync(string nome)
        {
            return await _context.Filiais
                                 .CountAsync(f => EF.Functions.Like(f.Nome, nome)) > 0;
        }

        // --- MÉTODO AUXILIAR ---

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Filial>> GetByNomeAsync(string nome)
        {
            return await _context.Filiais
                .Where(f => EF.Functions.Like(f.Nome.ToLower(), $"%{nome.ToLower()}%"))
                .ToListAsync();
        }
    }
}