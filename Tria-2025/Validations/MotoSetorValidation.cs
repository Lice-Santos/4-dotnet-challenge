using Tria_2025.DTO.MotoSetor;
using Tria_2025.Exceptions;
using Tria_2025.Repository;

namespace Tria_2025.Validations
{
    public class MotoSetorValidation
    {
        private readonly IMotoRepository _motoRepository;
        private readonly ISetorRepository _setorRepository;

        // Injeção de dependência dos repositórios de entidades relacionadas
        public MotoSetorValidation(IMotoRepository motoRepository, ISetorRepository setorRepository)
        {
            _motoRepository = motoRepository;
            _setorRepository = setorRepository;
        }

        public async Task ValidateCreateAsync(MotoSetorDTO dto)
        {
            // Validações Básicas de Negócio
            if (dto.Data == default)
            {
                throw new CampoVazioException(nameof(dto.Data));
            }

            // 1. REGRA CRÍTICA: Existência da Moto (Checagem da Chave Estrangeira)
            // Chamamos o método GetByIdAsync do IMotoRepository
            var moto = await _motoRepository.GetByIdAsync(dto.IdMoto);

            if (moto == null)
            {
                // Lança ObjetoNaoEncontradoException se a Moto não existir (código 404)
                throw new ObjetoNaoEncontradoException("Moto", dto.IdMoto);
            }

            // 2. REGRA CRÍTICA: Existência do Setor (Checagem da Chave Estrangeira)
            // Chamamos o método GetByIdAsync do ISetorRepository
            var setor = await _setorRepository.GetByIdAsync(dto.IdSetor);

            if (setor == null)
            {
                // Lança ObjetoNaoEncontradoException se o Setor não existir (código 404)
                throw new ObjetoNaoEncontradoException("Setor", dto.IdSetor);
            }

            // 3. Regra Opcional: Impedir Duplicidade de Associação (Uma moto pode estar em um setor por vez?)
            // Se uma Moto só pode ser associada a um Setor se não tiver uma associação ativa, 
            // você precisaria de um método no MotoSetorRepository para checar isso.
            // Ex: if (await _motoSetorRepository.IsMotoAlreadyInUse(dto.IdMoto)) { ... }
        }
    }
}