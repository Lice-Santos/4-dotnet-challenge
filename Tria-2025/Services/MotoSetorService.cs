using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO.MotoSetor;
using System.Linq;

namespace Tria_2025.Services
{
    public class MotoSetorService
    {
        private readonly IMotoSetorRepository _repository;
        private readonly MotoSetorValidation _validation;
        private readonly IMotoRepository _motoRepository;

        public MotoSetorService(IMotoSetorRepository repository, MotoSetorValidation validation, IMotoRepository motoRepository)
        {
            _repository = repository;
            _validation = validation;
            _motoRepository = motoRepository;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<MotoSetor> CreateMotoSetorAsync(MotoSetorDTO dto)
        {

            await _validation.ValidateCreateAsync(dto); 

            var novaAssociacao = new MotoSetor
            {
                Data = dto.Data,
                Fonte = dto.Fonte,
                IdMoto = dto.IdMoto,
                IdSetor = dto.IdSetor
            };

            return await _repository.AddAsync(novaAssociacao);
        }

        public async Task UpdateMotoSetorAsync(int id, MotoSetorDTO dto)
        {
            var associacaoExistente = await GetMotoSetorByIdAsync(id);


            await _validation.ValidateCreateAsync(dto);

            associacaoExistente.Data = dto.Data;
            associacaoExistente.Fonte = dto.Fonte;
            associacaoExistente.IdMoto = dto.IdMoto;
            associacaoExistente.IdSetor = dto.IdSetor;

            await _repository.UpdateAsync(associacaoExistente);
        }

        public async Task DeleteMotoSetorAsync(int id)
        {
            var associacao = await GetMotoSetorByIdAsync(id);

            await _repository.DeleteAsync(associacao);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<IEnumerable<MotoSetor>> GetAllMotoSetoresAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MotoSetor> GetMotoSetorByIdAsync(int id)
        {
            var associacao = await _repository.GetByIdAsync(id);

            if (associacao == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(MotoSetor), id);
            }
            return associacao;
        }

        public async Task<IEnumerable<MotoSetor>> GetMotoSetorByPlacaAsync(string placa)
        {
            var moto = await _motoRepository.GetByPlacaAsync(placa); 

            if (moto == null)
            {
                throw new ObjetoNaoEncontradoException("Moto", $"placa {placa}");
            }

            var todasAssociacoes = await _repository.GetAllAsync();

            var registrosEncontrados = todasAssociacoes.Where(ms => ms.IdMoto == moto.Id).ToList();

            if (!registrosEncontrados.Any())
            {
                throw new ObjetoNaoEncontradoException("Registro MotoSetor", $"placa {placa}");
            }

            return registrosEncontrados;
        }
    }
}

