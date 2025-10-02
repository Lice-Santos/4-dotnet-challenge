using Tria_2025.DTO.Funcionario;
using Tria_2025.Exceptions;
using Tria_2025.Models;
using Tria_2025.Repository;
using Tria_2025.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Tria_2025.Services
{
    public class FuncionarioService
    {
        private readonly IFuncionarioRepository _repository;
        private readonly FuncionarioValidation _validation;

        public FuncionarioService(IFuncionarioRepository repository, FuncionarioValidation validation)
        {
            _repository = repository;
            _validation = validation;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Funcionario> CreateFuncionarioAsync(FuncionarioDTO dto)
        {
            await _validation.ValidateCreateAsync(dto);

            var newFuncionario = new Funcionario
            {
                Nome = dto.Nome,
                Cargo = dto.Cargo,
                Email = dto.Email,
                Senha = dto.Senha
            };




            return await _repository.AddAsync(newFuncionario);
        }

        public async Task UpdateFuncionarioAsync(int id, FuncionarioDTO dto)
        {
            var existingFuncionario = await GetFuncionarioByIdAsync(id);

            await _validation.ValidateUpdateAsync(id, dto, existingFuncionario.Email);

            existingFuncionario.Nome = dto.Nome;
            existingFuncionario.Cargo = dto.Cargo;
            existingFuncionario.Email = dto.Email;
            existingFuncionario.Senha = dto.Senha; 

            await _repository.UpdateAsync(existingFuncionario);
        }

        public async Task DeleteFuncionarioAsync(int id)
        {
            var funcionario = await GetFuncionarioByIdAsync(id);


            await _repository.DeleteAsync(funcionario);
        }

        // --- MÉTODOS DE LEITURA ---

        public async Task<Funcionario> GetFuncionarioByIdAsync(int id)
        {
            var funcionario = await _repository.GetByIdAsync(id);
            if (funcionario == null)
            {
                throw new ObjetoNaoEncontradoException(nameof(Funcionario), id);
            }
            return funcionario;
        }

        public async Task<IEnumerable<Funcionario>> GetAllFuncionariosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Funcionario>> GetFuncionariosByNomeAsync(string nome)
        {

            var todos = await _repository.GetAllAsync();
            return todos.Where(f => f.Nome.ToLower().Contains(nome.ToLower()));
        }

        public async Task<IEnumerable<Funcionario>> GetFuncionariosByCargoAsync(string cargo)
        {
            var todos = await _repository.GetAllAsync();
            return todos.Where(f => f.Cargo.ToLower().Contains(cargo.ToLower()));
        }

        /// <summary>
        /// Simula a autenticação, procurando por e-mail e senha correspondentes.
        /// </summary>
        /// <returns>O nome do funcionário autenticado.</returns>
        public async Task<string> AuthenticateAsync(string email, string senha)
        {
            var funcionario = await _repository.GetByEmailAndSenhaAsync(email, senha); 

            if (funcionario == null)
            {
                throw new ObjetoNaoEncontradoException("Usuário ou senha", "Login");
            }

            return funcionario.Nome;
        }
    }
}
