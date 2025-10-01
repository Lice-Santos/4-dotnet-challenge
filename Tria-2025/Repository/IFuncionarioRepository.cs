using Tria_2025.Models;

namespace Tria_2025.Repository
{
    public interface IFuncionarioRepository
    {
        // CRUD Básico
        Task<Funcionario> AddAsync(Funcionario funcionario);
        Task UpdateAsync(Funcionario funcionario);
        Task DeleteAsync(Funcionario funcionario);

        // Métodos de Leitura
        Task<Funcionario> GetByIdAsync(int id);
        Task<IEnumerable<Funcionario>> GetAllAsync();

        // Método Específico para Validação (Unicidade de Email)
        Task<bool> EmailExistsAsync(string email);

        // MÉTODO ADICIONADO: Busca para autenticação
        Task<Funcionario> GetByEmailAndSenhaAsync(string email, string senha);

        // Para controle transacional
        Task<bool> SaveChangesAsync();
    }
}
