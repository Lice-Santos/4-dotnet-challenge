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
        public async Task<ActionResult<IEnumerable<Funcionario>>> Get()
        {
            var funcionarios = await _service.GetAllFuncionariosAsync();
            return Ok(funcionarios);
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca um funcionário específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do funcionário.</param>
        /// <returns>O objeto Funcionario solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
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
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, [FromBody] FuncionarioDTO dto)
        {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }
             
             try
             {
                 // O Service fará a validação de unicidade de e-mail e a checagem do ID
                 await _service.UpdateFuncionarioAsync(idPassado, dto);
                 return NoContent();
             }
             // Exceções de objeto não encontrado (404)
             catch (ObjetoNaoEncontradoException ex)
             {
                 return NotFound(new { Message = ex.Message });
             }
             // Exceções de validação (400)
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
        /// <returns>O novo funcionário criado ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FuncionarioDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação de Unicidade e Cargo
                var novoFuncionario = await _service.CreateFuncionarioAsync(dto);
                
                // Retorna 201 Created
                return CreatedAtAction(nameof(Get), new { id = novoFuncionario.Id }, novoFuncionario);
            }
            // Captura as exceções de validação de regra de negócio (400 Bad Request)
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
                return BadRequest(new { erro = ex.Message });
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
