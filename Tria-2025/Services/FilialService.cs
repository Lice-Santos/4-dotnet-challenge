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

        // O Service precisa do Repositório (DB) e da Validação (Regras)
        public FilialService(IFilialRepository repository, FilialValidation validation)
        {
            _repository = repository;
            _validation = validation;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Filial> CreateFilialAsync(FilialDTO dto)
        {
            // 1. Executa as validações (unicidade de nome e existência de IdEndereco)
            await _validation.ValidateCreateAsync(dto);

            // 2. Converte DTO para Model
            var novaFilial = new Filial
            {
                Nome = dto.Nome,
                IdEndereco = dto.IdEndereco
            };

            // 3. Salva no Repositório
            return await _repository.AddAsync(novaFilial);
        }

        public async Task UpdateFilialAsync(int id, FilialDTO dto)
        {
            // 1. Busca a Filial Existente
            var filialExistente = await _repository.GetByIdAsync(id);

            if (filialExistente == null)
            {
                // Lança 404
                throw new ObjetoNaoEncontradoException(nameof(Filial), id);
            }

            // 2. Executa validações de Update: Passa o objeto original para checar a unicidade do nome
            // Isso previne que a Filial falhe na validação de nome se ela mesma for dona do nome.
            await _validation.ValidateUpdateAsync(id, dto, filialExistente); // <-- CHAMADA CORRIGIDA!

            // 3. Aplica as alterações
            filialExistente.Nome = dto.Nome;
            filialExistente.IdEndereco = dto.IdEndereco;

            // 4. Salva no Repositório
            await _repository.UpdateAsync(filialExistente);
        }

        public async Task DeleteFilialAsync(int id)
        {
            var filial = await _repository.GetByIdAsync(id);

            if (filial == null)
            {
                // Lança 404
                throw new ObjetoNaoEncontradoException(nameof(Filial), id);
            }

            // Aqui é onde você adicionaria a lógica para checar se existem chaves estrangeiras pendentes (ex: motos ativas)
            // if (await _repository.HasRelatedEntities(id)) { throw new BadRequestException("Não é possível deletar filial com motos ativas."); }

            await _repository.DeleteAsync(filial);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Filial> GetFilialByIdAsync(int id)
        {
            var filial = await _repository.GetByIdAsync(id);

            if (filial == null)
            {
                // Lança 404
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
            // Note: O repositório já faz o ToLower().Contains()
            return await _repository.GetByNomeAsync(nome);
        }
    }
}
