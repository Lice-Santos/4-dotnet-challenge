using Tria_2025.DTO;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Adicionado para métodos de lista

namespace Tria_2025.Services
{
    public class SetorService
    {
        private readonly ISetorRepository _repository;
        private readonly SetorValidation _validation;

        public SetorService(ISetorRepository repository, SetorValidation validation)
        {
            _repository = repository;
            _validation = validation;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Setor> CreateSetorAsync(SetorDTO dto)
        {
            // 1. Executa as validações (unicidade)
            await _validation.ValidateCreateAsync(dto); // Lança CampoJaExistenteException (400)

            // 2. Converte DTO para Model
            var newSetor = new Setor { Nome = dto.Nome };

            // 3. Salva no Repositório
            return await _repository.AddAsync(newSetor);
        }

        public async Task UpdateSetorAsync(int id, SetorDTO dto)
        {
            // 1. Busca o objeto original (lança 404 se não existir)
            var setorExistente = await GetSetorByIdAsync(id);

            // 2. Executa validações de Update (checa se o novo nome já existe em OUTRO setor)
            // A validação de unicidade é inteligente e permite o nome original.
            await _validation.ValidateUpdateAsync(id, dto, setorExistente.Nome);

            // 3. Aplica as alterações
            setorExistente.Nome = dto.Nome;

            // 4. Salva no Repositório
            await _repository.UpdateAsync(setorExistente);
        }

        public async Task DeleteSetorAsync(int id)
        {
            // Busca o objeto (lança 404 se não existir)
            var setor = await GetSetorByIdAsync(id);

            // Nota: Em produção, você checaria aqui a integridade referencial (se há MotoSetor ligado a este Setor)

            await _repository.DeleteAsync(setor);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Setor> GetSetorByIdAsync(int id)
        {
            var setor = await _repository.GetByIdAsync(id);

            if (setor == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Setor), id);
            }
            return setor;
        }

        public async Task<IEnumerable<Setor>> GetAllSetoresAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
