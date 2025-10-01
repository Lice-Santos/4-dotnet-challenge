using Tria_2025.DTO.Funcionario;
using Tria_2025.Exceptions;
using Tria_2025.Repository;
using System.Linq;

namespace Tria_2025.Validations
{
    // Classe de Validação para Regras de Negócio e Checagens de DB (Unicidade/Existência)
    public class FuncionarioValidation
    {
        private readonly IFuncionarioRepository _repository;

        // Injeção de dependência do Repositório
        public FuncionarioValidation(IFuncionarioRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Valida as regras de negócio para a CRIAÇÃO de um novo funcionário.
        /// </summary>
        public async Task ValidateCreateAsync(FuncionarioDTO dto)
        {
            // 1. Regra de Negócio: E-mail Único (Assíncrono)
            var testeValidation = await _repository.EmailExistsAsync(dto.Email);
            if (testeValidation)
            {
                throw new CampoJaExistenteException(nameof(dto.Email), dto.Email);
            }

            // 2. Regra de Negócio: Cargos Permitidos (Valores Fixos)
            ValidateCargo(dto.Cargo);

        }

        /// <summary>
        /// Valida as regras de negócio para a ATUALIZAÇÃO de um funcionário.
        /// </summary>
        /// <param name="id">ID do funcionário a ser atualizado.</param>
        /// <param name="dto">Dados novos.</param>
        /// <param name="originalEmail">E-mail original do objeto antes da atualização.</param>
        public async Task ValidateUpdateAsync(int id, FuncionarioDTO dto, string originalEmail)
        {
            // 1. Checa a Unicidade do E-mail APENAS se o e-mail mudou
            if (!string.Equals(dto.Email, originalEmail, StringComparison.OrdinalIgnoreCase))
            {
                // Se o e-mail mudou, verificamos se o novo e-mail já existe no DB
                if (await _repository.EmailExistsAsync(dto.Email))
                {
                    throw new CampoJaExistenteException(nameof(dto.Email), dto.Email);
                }
            }

            // 2. Regra de Negócio: Cargos Permitidos
            //ValidateCargo(dto.Cargo);
        }

        /// <summary>
        /// Método auxiliar para validar se o cargo está na lista permitida.
        /// </summary>
        /// <param name="cargo">O cargo a ser validado.</param>
        /// <exception cref="CampoVazioException">Se o cargo for vazio.</exception>
        private void ValidateCargo(string cargo)
        {

            bool test3 = string.IsNullOrWhiteSpace(cargo);

            if (test3)
            {
                throw new CampoVazioException(nameof(cargo));
            }
        }
    }
}
