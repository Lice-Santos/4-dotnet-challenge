using Tria_2025.DTO.MotoSetor;
using Tria_2025.Exceptions;
using Tria_2025.Repository;

namespace Tria_2025.Validations
{
    public class MotoSetorValidation
    {
        private readonly IMotoRepository _motoRepository;
        private readonly ISetorRepository _setorRepository;

        public MotoSetorValidation(IMotoRepository motoRepository, ISetorRepository setorRepository)
        {
            _motoRepository = motoRepository;
            _setorRepository = setorRepository;
        }

        public async Task ValidateCreateAsync(MotoSetorDTO dto)
        {
            if (dto.Data == default)
            {
                throw new CampoVazioException(nameof(dto.Data));
            }


            var moto = await _motoRepository.GetByIdAsync(dto.IdMoto);

            if (moto == null)
            {
                throw new ObjetoNaoEncontradoException("Moto", dto.IdMoto);
            }

            var setor = await _setorRepository.GetByIdAsync(dto.IdSetor);

            if (setor == null)
            {
                throw new ObjetoNaoEncontradoException("Setor", dto.IdSetor);
            }


        }
    }
}