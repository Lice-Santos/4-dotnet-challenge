using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Services;
using Tria_2025.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO;

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pelo CRUD da entidade Setor.
    /// Delega a lógica de negócio e validação de unicidade do nome ao SetorService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SetorController : ControllerBase
    {
        private readonly SetorService _service;

        /// <summary>
        /// Construtor que injeta o serviço de Setor.
        /// </summary>
        /// <param name="service">O serviço de Setor para orquestração.</param>
        public SetorController(SetorService service)
        {
            _service = service;
        }

        // --- GET ALL ---
        /// <summary>
        /// Obtém a lista completa de todos os setores cadastrados.
        /// </summary>
        /// <returns>Uma lista de objetos Setor.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Setor>>> Get()
        {
            var setores = await _service.GetAllSetoresAsync();
            return Ok(setores);
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca um setor específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do setor.</param>
        /// <returns>O objeto Setor solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Setor>> Get(int id)
        {
            try
            {
                var setor = await _service.GetSetorByIdAsync(id);
                return Ok(setor);
            }
            // Captura a exceção de Not Found lançada pelo Service
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar o setor." });
            }
        }

        // --- PUT (ATUALIZAÇÃO) ---
        /// <summary>
        /// Atualiza as informações de um setor existente.
        /// </summary>
        /// <param name="idPassado">O ID do setor a ser atualizado (da URL).</param>
        /// <param name="setorDto">O DTO com o novo nome.</param>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, [FromBody] SetorDTO setorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação de unicidade do nome e a checagem do ID
                await _service.UpdateSetorAsync(idPassado, setorDto);
                return NoContent();
            }
            // Exceções de objeto não encontrado (404)
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            // Exceções de validação (400) - Nome já existe ou tamanho inválido
            catch (CampoJaExistenteException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (TamanhoInvalidoDeCaracteresException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao atualizar o setor." });
            }
        }

        // --- POST (CRIAÇÃO) ---
        /// <summary>
        /// Cria um novo setor, verificando a unicidade do nome.
        /// </summary>
        /// <param name="setorDto">Dados do Setor (Nome).</param>
        /// <returns>O novo setor criado ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SetorDTO setorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação de Unicidade
                var novoSetor = await _service.CreateSetorAsync(setorDto);

                // Retorna 201 Created
                return CreatedAtAction(nameof(Get), new { id = novoSetor.Id }, novoSetor);
            }
            // Captura a exceção de validação de regra de negócio (400 Bad Request)
            catch (CampoJaExistenteException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (TamanhoInvalidoDeCaracteresException ex)
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
        /// Exclui um setor permanentemente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do setor a ser excluído.</param>
        /// <returns>204 No Content ou 404 Not Found.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteSetorAsync(id);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao deletar o setor." });
            }
        }
    }
}
