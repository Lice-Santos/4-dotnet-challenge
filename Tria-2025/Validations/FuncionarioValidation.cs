using Tria_2025.DTO.Funcionario;
using Tria_2025.Exceptions;
using Tria_2025.Repository;
using System.Linq;

namespace Tria_2025.Validations
{
    public class FuncionarioValidation
    {
        private readonly IFuncionarioRepository _repository;

        public FuncionarioValidation(IFuncionarioRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Valida as regras de negócio para a CRIAÇÃO de um novo funcionário.
        /// </summary>
        public async Task ValidateCreateAsync(FuncionarioDTO dto)
        {
            var testeValidation = await _repository.EmailExistsAsync(dto.Email);
            if (testeValidation)
            {
                throw new CampoJaExistenteException(nameof(dto.Email), dto.Email);
            }

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
            if (!string.Equals(dto.Email, originalEmail, StringComparison.OrdinalIgnoreCase))
            {
                if (await _repository.EmailExistsAsync(dto.Email))
                {
                    throw new CampoJaExistenteException(nameof(dto.Email), dto.Email);
                }
            }

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
