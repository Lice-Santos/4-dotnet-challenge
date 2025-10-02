using Tria_2025.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tria_2025.Repository
{
    public interface IFilialRepository
    {
        // CRUD Básico
        Task<Filial> AddAsync(Filial filial);
        Task UpdateAsync(Filial filial);
        Task DeleteAsync(Filial filial);

        // Métodos de Leitura
        Task<Filial> GetByIdAsync(int id);
        Task<IEnumerable<Filial>> GetAllAsync();

        // Método Específico para Validação
        Task<bool> NomeFilialExistsAsync(string nome);

        // NOVO: Método para buscar por nome 
        Task<IEnumerable<Filial>> GetByNomeAsync(string nome);

        // Para controle transacional
        Task<bool> SaveChangesAsync();



    }
}
