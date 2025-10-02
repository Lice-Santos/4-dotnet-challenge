using Tria_2025.Exceptions;
using Tria_2025.Repository;
using Tria_2025.DTO;


namespace Tria_2025.Validations
{
    public class SetorValidation
    {
        private readonly ISetorRepository _repository;

        public SetorValidation(ISetorRepository repository)
        {
            _repository = repository;
        }

        public async Task ValidateCreateAsync(SetorDTO dto)
        {
            if (await _repository.NomeSetorExistsAsync(dto.Nome))
            {
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
            ValidateNome(dto.Nome);


            if (!dto.Nome.Equals(originalNome, StringComparison.OrdinalIgnoreCase))
            {
                if (await _repository.NomeSetorExistsAsync(dto.Nome))
                {
                    throw new CampoJaExistenteException(nameof(dto.Nome), dto.Nome);
                }
            }
        }

        // --- MÉTODOS AUXILIARES ---

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
