using Tria_2025.DTOs.Moto;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Tria_2025.Services
{
    public class MotoService
    {
        private readonly IMotoRepository _repository;
        private readonly MotoValidation _validation;

        public MotoService(IMotoRepository repository, MotoValidation validation)
        {
            _repository = repository;
            _validation = validation;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Moto> CreateMotoAsync(MotoDTO dto)
        {

            await _validation.ValidateCreateAsync(dto);

            var novaMoto = new Moto
            {
                Placa = dto.Placa.ToUpper().Trim(), 
                Modelo = dto.Modelo,
                Ano = dto.Ano,
                TipoCombustivel = dto.TipoCombustivel,
                IdFilial = dto.IdFilial
            };

            return await _repository.AddAsync(novaMoto);
        }

        public async Task UpdateMotoAsync(int id, MotoDTO dto)
        {
            var existingMoto = await GetMotoByIdAsync(id);

            await _validation.ValidateUpdateAsync(id, dto, existingMoto);

            existingMoto.Placa = dto.Placa.ToUpper().Trim();
            existingMoto.Modelo = dto.Modelo;
            existingMoto.Ano = dto.Ano;
            existingMoto.TipoCombustivel = dto.TipoCombustivel;
            existingMoto.IdFilial = dto.IdFilial;

            await _repository.UpdateAsync(existingMoto);
        }

        public async Task DeleteMotoAsync(int id)
        {
            var moto = await GetMotoByIdAsync(id);


            await _repository.DeleteAsync(moto);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<IEnumerable<Moto>> GetAllMotosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Moto> GetMotoByIdAsync(int id)
        {
            var moto = await _repository.GetByIdAsync(id);

            if (moto == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Moto), id);
            }
            return moto;
        }

        public async Task<Moto> GetMotoByPlacaAsync(string placa)
        {

            var todasMotos = await _repository.GetAllAsync();
            var moto = todasMotos.FirstOrDefault(m => m.Placa.ToUpper().Trim() == placa.ToUpper().Trim());

            if (moto == null)
            {
                throw new ObjetoNaoEncontradoException("Moto pela Placa", placa);
            }
            return moto;
        }

        public async Task<IEnumerable<Moto>> GetMotosAcimaDoAnoAsync(int ano)
        {
            var todasMotos = await _repository.GetAllAsync();
            return todasMotos.Where(m => m.Ano >= ano).ToList();
        }

        public async Task<IEnumerable<Moto>> GetMotosByModeloAsync(string modelo)
        {
            var todasMotos = await _repository.GetAllAsync();
            return todasMotos.Where(m => m.Modelo.ToLower().Contains(modelo.ToLower())).ToList();
        }
    }
}
