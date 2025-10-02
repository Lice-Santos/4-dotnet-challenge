using Tria_2025.DTO;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO;

namespace Tria_2025.Services
{
    public class FilialService
    {
        private readonly IFilialRepository _repository;
        private readonly FilialValidation _validation;

        public FilialService(IFilialRepository repository, FilialValidation validation)
        {
            _repository = repository;
            _validation = validation;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Filial> CreateFilialAsync(FilialDTO dto)
        {
            await _validation.ValidateCreateAsync(dto);

            var novaFilial = new Filial
            {
                Nome = dto.Nome,
                IdEndereco = dto.IdEndereco
            };

            return await _repository.AddAsync(novaFilial);
        }

        public async Task UpdateFilialAsync(int id, FilialDTO dto)
        {
            var filialExistente = await _repository.GetByIdAsync(id);

            if (filialExistente == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Filial), id);
            }


            await _validation.ValidateUpdateAsync(id, dto, filialExistente); 

            filialExistente.Nome = dto.Nome;
            filialExistente.IdEndereco = dto.IdEndereco;

            await _repository.UpdateAsync(filialExistente);
        }

        public async Task DeleteFilialAsync(int id)
        {
            var filial = await _repository.GetByIdAsync(id);

            if (filial == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Filial), id);
            }



            await _repository.DeleteAsync(filial);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Filial> GetFilialByIdAsync(int id)
        {
            var filial = await _repository.GetByIdAsync(id);

            if (filial == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Filial), id);
            }
            return filial;
        }

        public async Task<IEnumerable<Filial>> GetAllFiliaisAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Filial>> GetFiliaisByNomeAsync(string nome)
        {
            return await _repository.GetByNomeAsync(nome);
        }
    }
}
