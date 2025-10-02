using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public class MotoRepository : IMotoRepository
    {
        private readonly AppDbContext _context;

        public MotoRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS CRUD BÁSICOS ---

        public async Task<Moto> AddAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await SaveChangesAsync();
            return moto;
        }

        public async Task UpdateAsync(Moto moto)
        {
            _context.Motos.Attach(moto);
            _context.Entry(moto).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Moto moto)
        {
            _context.Motos.Remove(moto);
            await SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Moto> GetByIdAsync(int id)
        {
            return await _context.Motos.Include(m => m.Filial).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Moto>> GetAllAsync()
        {
            return await _context.Motos.ToListAsync();
        }

        // --- MÉTODO ESPECÍFICO PARA VALIDAÇÃO ---


        public async Task<bool> PlacaExistsAsync(string placa)
        {
            string placaNormalized = placa.ToUpper().Trim();


            return await _context.Motos.CountAsync(m => m.Placa.ToUpper() == placaNormalized) > 0;
        }


        // --- MÉTODO AUXILIAR ---

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<Moto> GetByPlacaAsync(string placa)
        {
            throw new NotImplementedException();
        }
    }
}