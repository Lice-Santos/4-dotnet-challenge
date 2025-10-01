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

        // Injeção do Repositório (acesso ao DB) e da Validação (regras de negócio)
        public FuncionarioService(IFuncionarioRepository repository, FuncionarioValidation validation)
        {
            _repository = repository;
            _validation = validation;
        }

        // --- MÉTODOS DE ESCRITA ---

        public async Task<Funcionario> CreateFuncionarioAsync(FuncionarioDTO dto)
        {
            // 1. Executa as validações de regras de negócio (Unicidade e Cargo)
            await _validation.ValidateCreateAsync(dto);

            // 2. Converte o DTO para o Model
            var newFuncionario = new Funcionario
            {
                Nome = dto.Nome,
                Cargo = dto.Cargo,
                Email = dto.Email,
                Senha = dto.Senha
            };

            //var novoFuncionarioteste = new Funcionario { Id = 10, Nome = "Marcos", Cargo = "ceo", Email = "marcus@gmauil.com", Senha = "fdafs23324"};



            // 3. Salva no Repositório
            return await _repository.AddAsync(newFuncionario);
        }

        public async Task UpdateFuncionarioAsync(int id, FuncionarioDTO dto)
        {
            // 1. Busca o objeto original (lança 404 se não existir)
            var existingFuncionario = await GetFuncionarioByIdAsync(id);

            // 2. Validações de Update (checa se o novo email já existe em OUTRO funcionário)
            await _validation.ValidateUpdateAsync(id, dto, existingFuncionario.Email);

            // 3. Aplica as alterações
            existingFuncionario.Nome = dto.Nome;
            existingFuncionario.Cargo = dto.Cargo;
            existingFuncionario.Email = dto.Email;
            existingFuncionario.Senha = dto.Senha; // Sempre atualiza a senha (em produção, hash aqui)

            // 4. Salva no Repositório
            await _repository.UpdateAsync(existingFuncionario);
        }

        public async Task DeleteFuncionarioAsync(int id)
        {
            var funcionario = await GetFuncionarioByIdAsync(id);
            // Se GetFuncionarioByIdAsync retornar null, ele lança ObjetoNaoEncontradoException (404)

            // Em um projeto real, você faria checagens de integridade referencial aqui (ex: checar se há registros em MotoSetor)

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
            // Nota: Se o Repositório tiver um método GetByNome, ele é usado. Caso contrário, 
            // filtramos todos aqui (se o GetAll for rápido, o que não é o ideal).
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
            var funcionario = await _repository.GetByEmailAndSenhaAsync(email, senha); // Método assumido no Repositório

            if (funcionario == null)
            {
                // Lança 404 para o Controller, que trata como falha de login
                throw new ObjetoNaoEncontradoException("Usuário ou senha", "Login");
            }

            return funcionario.Nome;
        }
    }
}
