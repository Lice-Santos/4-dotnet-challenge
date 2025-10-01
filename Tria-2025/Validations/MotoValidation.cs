using Tria_2025.DTOs.Moto;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Numerics;

namespace Tria_2025.Validations
{
    public class MotoValidation
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IFilialRepository _filialRepository;

        public MotoValidation(IMotoRepository motoRepository, IFilialRepository filialRepository)
        {
            _motoRepository = motoRepository;
            _filialRepository = filialRepository;
        }

        public async Task ValidateCreateAsync(MotoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Placa))
                throw new CampoVazioException(dto.Placa);

            //1.Regra de Negócio: Placa Única
            if (await _motoRepository.PlacaExistsAsync(dto.Placa))
            {
                throw new CampoJaExistenteException(nameof(dto.Placa), dto.Placa);
            }

            // 2. Regra de Negócio: Tipo de Combustível Válido
            ValidateTipoCombustivel(dto.TipoCombustivel);

            // 3. REGRA CRÍTICA: Existência da Filial
            await ValidateFilialExistsAsync(dto.IdFilial);
        }



        public async Task ValidateUpdateAsync(int id, MotoDTO dto, Moto originalMoto)
        {
            // 1. Regra de Negócio: Placa Única (Checagem inteligente)
            // Normaliza a placa nova para comparação
            string newPlacaNormalized = dto.Placa.ToUpper().Trim();

            // Se a placa foi alterada E o novo valor já existe no DB
            if (newPlacaNormalized != originalMoto.Placa.ToUpper().Trim() &&
                await _motoRepository.PlacaExistsAsync(newPlacaNormalized))
            {
                throw new CampoJaExistenteException(nameof(dto.Placa), dto.Placa);
            }

            // 2. Regra de Negócio: Tipo de Combustível Válido
            ValidateTipoCombustivel(dto.TipoCombustivel);

            // 3. REGRA CRÍTICA: Existência da Filial
            await ValidateFilialExistsAsync(dto.IdFilial);
        }

        // --- MÉTODOS AUXILIARES ---

        private static void ValidateTipoCombustivel(string tipo)
        {
            var tiposPermitidos = new[] { "Gasolina", "Etanol", "Flex" };
            if (!tiposPermitidos.Contains(tipo, StringComparer.OrdinalIgnoreCase))
            {
                throw new CampoInvalidoException("Tipo de gasolina");
            }
        }

        private async Task ValidateFilialExistsAsync(int idFilial)
        {
            var filial = await _filialRepository.GetByIdAsync(idFilial);

            if (filial == null)
            {
                throw new ObjetoNaoEncontradoException("Filial", idFilial);
            }
        }
    }
}
