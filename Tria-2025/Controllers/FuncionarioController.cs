using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Services;
using Tria_2025.Exceptions;
using Tria_2025.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO.Funcionario;

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pelo CRUD e operações de Funcionário.
    /// Delega a lógica de negócio (validação de e-mail e cargo) ao FuncionarioService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioService _service;

        /// <summary>
        /// Construtor que injeta o serviço de Funcionário.
        /// </summary>
        /// <param name="service">O serviço de Funcionário para orquestração.</param>
        public FuncionarioController(FuncionarioService service)
        {
            _service = service;
        }

        // --- GET ALL ---
        /// <summary>
        /// Obtém a lista completa de todos os funcionários cadastrados.
        /// </summary>
        /// <returns>Uma lista de objetos Funcionario.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Funcionario>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                     // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<IEnumerable<Funcionario>>> Get()
        {
            try
            {
                var funcionarios = await _service.GetAllFuncionariosAsync();
                return Ok(funcionarios);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a lista de funcionários." });
            }
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca um funcionário específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do funcionário.</param>
        /// <returns>O objeto Funcionario solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Funcionario))]       // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<Funcionario>> Get(int id)
        {
            try
            {
                var funcionario = await _service.GetFuncionarioByIdAsync(id);
                return Ok(funcionario);
            }
            // Captura a exceção de Not Found lançada pelo Service
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar o funcionário." });
            }
        }

        // --- GET POR NOME ---
        /// <summary>
        /// Busca funcionários que contenham o nome passado na URL.
        /// </summary>
        /// <param name="nomeFuncionario">O nome ou parte do nome do funcionário.</param>
        /// <returns>Lista de funcionários encontrados ou 404 Not Found.</returns>
        [HttpGet("nome/{nomeFuncionario}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Funcionario>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                             // ⭐️ Documenta não encontrado
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<List<Funcionario>>> BuscarFuncionarioPorNome(string nomeFuncionario)
        {
            var funcionarios = await _service.GetFuncionariosByNomeAsync(nomeFuncionario);

            if (funcionarios == null || !funcionarios.Any())
            {
                return NotFound("Nenhum funcionário encontrado com o nome especificado.");
            }

            return Ok(funcionarios);
        }

        // --- SIMULAÇÃO DE LOGIN ---
        /// <summary>
        /// Simula o processo de login verificando e-mail e senha.
        /// </summary>
        /// <param name="email">E-mail do funcionário.</param>
        /// <param name="senha">Senha do funcionário.</param>
        /// <returns>Mensagem de boas-vindas ou 404 Not Found.</returns>
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]           // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta login falho
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<string>> BuscarDadosParaLogin(string email, string senha)
        {
            try
            {
                var nomeFuncionario = await _service.AuthenticateAsync(email, senha);
                return Ok($"Seja bem vindo {nomeFuncionario}");
            }
            // Captura ObjetoNaoEncontradoException se o login falhar
            catch (ObjetoNaoEncontradoException ex)
            {
                // É padrão retornar 404 para credenciais inválidas (para não dar dica)
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno durante o login." });
            }
        }

        // --- GET POR CARGO ---
        /// <summary>
        /// Busca todos os funcionários que possuam o cargo passado.
        /// </summary>
        /// <param name="cargo">O cargo a ser pesquisado.</param>
        /// <returns>Lista de funcionários encontrados ou 404 Not Found.</returns>
        [HttpGet("cargo/{cargo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Funcionario>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                             // ⭐️ Documenta não encontrado
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<List<Funcionario>>> BuscarPorCargo(string cargo)
        {
            var funcionarios = await _service.GetFuncionariosByCargoAsync(cargo);

            if (funcionarios == null || !funcionarios.Any())
            {
                return NotFound("Nenhum funcionário encontrado para o cargo especificado.");
            }

            return Ok(funcionarios);
        }

        // --- PUT (ATUALIZAÇÃO) ---
        /// <summary>
        /// Atualiza as informações de um funcionário existente.
        /// </summary>
        /// <param name="idPassado">O ID do funcionário a ser atualizado (da URL).</param>
        /// <param name="dto">O objeto DTO com as novas informações.</param>
        /// <remarks>
        /// Exemplo de Payload (para PUT /api/Funcionario/1):
        ///
        ///     PUT /api/Funcionario/1
        ///     {
        ///        "nome": "Pedro Álvares (Atualizado)",
        ///        "cargo": "Gerente",
        ///        "email": "pedro.alvares.novo@tria.com",
        ///        "senha": "NovaSenha123"
        ///     }
        /// </remarks>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{idPassado}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta validações (e-mail existente, campo inválido)
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Put(int idPassado, [FromBody] FuncionarioDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateFuncionarioAsync(idPassado, dto);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (CampoJaExistenteException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (CampoInvalidoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao atualizar o funcionário." });
            }
        }

        // --- POST (CRIAÇÃO) ---
        /// <summary>
        /// Cria um novo funcionário, verificando a unicidade do e-mail e a validade do cargo.
        /// </summary>
        /// <param name="dto">Dados do Funcionário (Nome, Cargo, Email, Senha).</param>
        /// <remarks>
        /// Exemplo de Payload:
        ///
        ///     POST /api/Funcionario
        ///     {
        ///        "nome": "João da Silva",
        ///        "cargo": "Vendedor",
        ///        "email": "joao.silva@tria.com",
        ///        "senha": "Senha123"
        ///     }
        /// </remarks>
        /// <returns>O novo funcionário criado ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Funcionario))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta todas as exceções de validação
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Post([FromBody] FuncionarioDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var novoFuncionario = await _service.CreateFuncionarioAsync(dto);

                return CreatedAtAction(nameof(Get), new { id = novoFuncionario.Id }, novoFuncionario);
            }
            catch (CampoJaExistenteException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (CampoInvalidoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (CampoVazioException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno", Detalhes = ex.Message });
            }
        }

        // --- DELETE ---
        /// <summary>
        /// Exclui um funcionário permanentemente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser excluído.</param>
        /// <returns>204 No Content ou 404 Not Found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteFuncionarioAsync(id);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao deletar o funcionário." });
            }
        }
    }
}