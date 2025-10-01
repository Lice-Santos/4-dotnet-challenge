using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public interface IMotoSetorRepository
    {
        // CRUD Básico
        Task<MotoSetor> AddAsync(MotoSetor motoSetor);
        Task UpdateAsync(MotoSetor motoSetor);
        Task DeleteAsync(MotoSetor motoSetor);

        // Métodos de Leitura
        Task<MotoSetor> GetByIdAsync(int id);
        Task<IEnumerable<MotoSetor>> GetAllAsync();

        // Para controle transacional
        Task<bool> SaveChangesAsync();
    }
}