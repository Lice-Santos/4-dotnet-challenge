using Tria_2025.Exceptions;
using Tria_2025.Repository;
using Tria_2025.DTO; // Usando o DTO com o namespace correto
using System.Threading.Tasks;
using System.Linq; // Adicionando importação que estava faltando na seleção de código

namespace Tria_2025.Validations
{
    public class SetorValidation
    {
        private readonly ISetorRepository _repository;

        // Injeção de dependência do Repositório
        public SetorValidation(ISetorRepository repository)
        {
            _repository = repository;
        }

        public async Task ValidateCreateAsync(SetorDTO dto)
        {
            // 1. Regra de Negócio: Nome do Setor Único (Assíncrono)
            if (await _repository.NomeSetorExistsAsync(dto.Nome))
            {
                // Lança CampoJaExistenteException
                throw new CampoJaExistenteException(nameof(dto.Nome), dto.Nome);
            }
        }

        /// <summary>
        /// Validação específica para a operação de PUT (Atualização).
        /// Garante que o nome do setor é único, mas permite o nome original.
        /// </summary>
        /// <param name="id">ID do setor sendo atualizado.</param>
        /// <param name="dto">DTO com os novos dados.</param>
        /// <param name="originalNome">Nome original do setor, carregado do banco.</param>
        public async Task ValidateUpdateAsync(int id, SetorDTO dto, string originalNome)
        {
            // 1. Checa as validações básicas de tamanho/vazio (se não estiverem no DTO)
            ValidateNome(dto.Nome);

            // 2. Lógica de Unicidade no Update:
            // Se o Nome foi alterado E o novo nome já existe no banco (pertencendo a OUTRO ID)
            if (!dto.Nome.Equals(originalNome, StringComparison.OrdinalIgnoreCase))
            {
                if (await _repository.NomeSetorExistsAsync(dto.Nome))
                {
                    // Se o nome existe e é diferente do nome original, lança exceção.
                    throw new CampoJaExistenteException(nameof(dto.Nome), dto.Nome);
                }
            }
        }

        // --- MÉTODOS AUXILIARES ---

        // Exemplo de validação de campo individual para um Update específico
        public void ValidateNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new CampoVazioException(nameof(nome));
            }

            if (nome.Length < 3 || nome.Length > 50)
            {
                throw new TamanhoInvalidoDeCaracteresException(nameof(nome), 50, 3);
            }
        }
    }
}
