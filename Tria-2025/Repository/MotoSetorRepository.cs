using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public class MotoSetorRepository : IMotoSetorRepository
    {
        private readonly AppDbContext _context;

        public MotoSetorRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS CRUD BÁSICOS ---

        public async Task<MotoSetor> AddAsync(MotoSetor motoSetor)
        {
            _context.Moto_Setores.Add(motoSetor); // Assumindo o DbSet chama 'Moto_Setores'
            await SaveChangesAsync();
            return motoSetor;
        }

        public async Task UpdateAsync(MotoSetor motoSetor)
        {
            _context.Moto_Setores.Attach(motoSetor);
            _context.Entry(motoSetor).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(MotoSetor motoSetor)
        {
            _context.Moto_Setores.Remove(motoSetor);
            await SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<MotoSetor> GetByIdAsync(int id)
        {
            // Inclui as referências para carregamento eager
            return await _context.Moto_Setores
                                 .Include(ms => ms.Moto)
                                 .Include(ms => ms.Setor)
                                 .FirstOrDefaultAsync(ms => ms.Id == id);
        }

        public async Task<IEnumerable<MotoSetor>> GetAllAsync()
        {
            return await _context.Moto_Setores.ToListAsync();
        }

        // --- MÉTODO AUXILIAR ---

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}