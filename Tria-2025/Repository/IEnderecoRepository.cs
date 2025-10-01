using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public interface IEnderecoRepository
    {
        // Métodos CRUD básicos
        Task<Endereco> AddAsync(Endereco endereco);
        Task UpdateAsync(Endereco endereco);
        Task DeleteAsync(Endereco endereco);

        // Métodos de Leitura
        Task<Endereco> GetByIdAsync(int id);
        Task<IEnumerable<Endereco>> GetAllAsync();

        // Métodos de Busca Específica
        Task<Endereco> GetByCepAsync(string cep);
        Task<IEnumerable<Endereco>> GetByLogradouroAsync(string logradouro);

        // Métodos de Checagem (úteis para Validações/Updates)
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> SaveChangesAsync(); // Para controle transacional
    }
}