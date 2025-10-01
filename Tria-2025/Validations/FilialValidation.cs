using Tria_2025.DTO;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;

namespace Tria_2025.Validations
{
    public class FilialValidation
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FilialValidation(IFilialRepository filialRepository, IEnderecoRepository enderecoRepository)
        {
            _filialRepository = filialRepository;
            _enderecoRepository = enderecoRepository;
        }

        /// <summary>
        /// Valida as regras de negócio para a criação de uma nova Filial.
        /// </summary>
        public async Task ValidateCreateAsync(FilialDTO dto)
        {

            // Validação 2: Existência da Chave Estrangeira IdEndereco
            await ValidateEnderecoExistsAsync(dto.IdEndereco);
        }

        /// <summary>
        /// Valida as regras de negócio para a atualização de uma Filial existente.
        /// Verifica se o nome foi alterado e, se sim, se já existe em outra Filial.
        /// </summary>
        /// <param name="id">ID da filial que está sendo atualizada.</param>
        /// <param name="dto">Dados de atualização.</param>
        /// <param name="originalFilial">Objeto Filial original antes das mudanças.</param>
        public async Task ValidateUpdateAsync(int id, FilialDTO dto, Filial originalFilial) // IMPLEMENTAÇÃO COMPLETA
        {
            // Validação 1: Unicidade do Nome (Lógica Correta)

            // Checa se o nome no DTO é diferente do nome original do objeto (ignorando case).
            if (!dto.Nome.Equals(originalFilial.Nome, StringComparison.OrdinalIgnoreCase))
            {
                // Se o nome foi alterado, checamos se o novo nome já existe no DB.
                if (await _filialRepository.NomeFilialExistsAsync(dto.Nome))
                {
                    throw new CampoJaExistenteException(nameof(dto.Nome), dto.Nome);
                }
            }
            // Se o nome não foi alterado, a validação de unicidade passa.

            // Validação 2: Existência da Chave Estrangeira IdEndereco
            await ValidateEnderecoExistsAsync(dto.IdEndereco);
        }

        /// <summary>
        /// Método auxiliar para checar se o Endereco existe.
        /// </summary>
        /// <param name="idEndereco">ID do endereço a ser verificado.</param>
        private async Task ValidateEnderecoExistsAsync(int idEndereco)
        {
            var endereco = await _enderecoRepository.GetByIdAsync(idEndereco);

            if (endereco == null)
            {
                // Lança ObjetoNaoEncontradoException se o Endereço referenciado não for encontrado
                throw new ObjetoNaoEncontradoException("Endereco", idEndereco);
            }
        }
    }
}
