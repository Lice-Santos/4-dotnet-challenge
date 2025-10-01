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
        private readonly IMotoRepository _motoRepository; // Adicionado para GetByPlaca

        public MotoSetorService(IMotoSetorRepository repository, MotoSetorValidation validation, IMotoRepository motoRepository)
        {
            _repository = repository;
            _validation = validation;
            _motoRepository = motoRepository;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<MotoSetor> CreateMotoSetorAsync(MotoSetorDTO dto)
        {
            // 1. Executa as validações (existência de IdMoto e IdSetor)
            await _validation.ValidateCreateAsync(dto); // Lança ObjetoNaoEncontradoException (400)

            // 2. Converte DTO para Model
            var novaAssociacao = new MotoSetor
            {
                Data = dto.Data,
                Fonte = dto.Fonte,
                IdMoto = dto.IdMoto,
                IdSetor = dto.IdSetor
            };

            // 3. Salva no Repositório
            return await _repository.AddAsync(novaAssociacao);
        }

        public async Task UpdateMotoSetorAsync(int id, MotoSetorDTO dto)
        {
            // 1. Verifica se a associação original existe (lança 404 se não)
            var associacaoExistente = await GetMotoSetorByIdAsync(id);

            // 2. Executa as validações das chaves estrangeiras (lança 400 se as FKs forem inválidas)
            // Reutilizamos o ValidateCreateAsync pois a lógica de FK é a mesma.
            await _validation.ValidateCreateAsync(dto);

            // 3. Aplica as alterações
            associacaoExistente.Data = dto.Data;
            associacaoExistente.Fonte = dto.Fonte;
            associacaoExistente.IdMoto = dto.IdMoto;
            associacaoExistente.IdSetor = dto.IdSetor;

            // 4. Salva no Repositório
            await _repository.UpdateAsync(associacaoExistente);
        }

        public async Task DeleteMotoSetorAsync(int id)
        {
            var associacao = await GetMotoSetorByIdAsync(id);
            // Se GetMotoSetorByIdAsync falhar, ele já lança ObjetoNaoEncontradoException

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
            // 1. Busca a Moto pelo Repositório da Moto
            var moto = await _motoRepository.GetByPlacaAsync(placa); // Assume-se que este método existe no IMotoRepository

            if (moto == null)
            {
                // Lança 404 se a moto não existir
                throw new ObjetoNaoEncontradoException("Moto", $"placa {placa}");
            }

            // 2. Busca todas as associações para aquela Moto
            var todasAssociacoes = await _repository.GetAllAsync();

            var registrosEncontrados = todasAssociacoes.Where(ms => ms.IdMoto == moto.Id).ToList();

            if (!registrosEncontrados.Any())
            {
                // Lança 404 se a associação não for encontrada (comportamento do Controller original)
                throw new ObjetoNaoEncontradoException("Registro MotoSetor", $"placa {placa}");
            }

            return registrosEncontrados;
        }
    }
}

