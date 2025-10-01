using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public interface ISetorRepository
    {
        // CRUD Básico
        Task<Setor> AddAsync(Setor setor);
        Task UpdateAsync(Setor setor);
        Task DeleteAsync(Setor setor);

        // Métodos de Leitura
        Task<Setor> GetByIdAsync(int id);
        Task<IEnumerable<Setor>> GetAllAsync();

        // Método Específico para Validação (Unicidade de Nome)
        Task<bool> NomeSetorExistsAsync(string nome);

        // Para controle transacional
        Task<bool> SaveChangesAsync();
    }
}