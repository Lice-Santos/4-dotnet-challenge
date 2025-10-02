using Tria_2025.DTO;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;

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


        public async Task<Setor> CreateSetorAsync(SetorDTO dto)
        {

            await _validation.ValidateCreateAsync(dto); 

            var newSetor = new Setor { Nome = dto.Nome };

            return await _repository.AddAsync(newSetor);
        }

        public async Task UpdateSetorAsync(int id, SetorDTO dto)
        {
            var setorExistente = await GetSetorByIdAsync(id);

            await _validation.ValidateUpdateAsync(id, dto, setorExistente.Nome);

            setorExistente.Nome = dto.Nome;

            await _repository.UpdateAsync(setorExistente);
        }

        public async Task DeleteSetorAsync(int id)
        {
            var setor = await GetSetorByIdAsync(id);

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
