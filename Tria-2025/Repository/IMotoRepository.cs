using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public interface IMotoRepository
    {
        // CRUD Básico
        Task<Moto> AddAsync(Moto moto);
        Task UpdateAsync(Moto moto);
        Task DeleteAsync(Moto moto);

        // Métodos de Leitura
        Task<Moto> GetByIdAsync(int id);
        Task<IEnumerable<Moto>> GetAllAsync();

        // Método Específico para Validação
        Task<bool> PlacaExistsAsync(string placa);

        // MÉTODO ADICIONADO: Busca uma moto pela placa
        Task<Moto> GetByPlacaAsync(string placa);

        // Para controle transacional
        Task<bool> SaveChangesAsync();
    }
}
