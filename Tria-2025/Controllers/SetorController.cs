using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Services;
using Tria_2025.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pelo CRUD da entidade Setor.
    /// Delega a lógica de negócio e validação de unicidade do nome ao SetorService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///    GET /Setor
        ///
        /// </remarks>
        /// <returns>Uma lista de objetos Setor.</returns>
        /// <response code="200"> Retorna a lista completa de produtos</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Setor>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                     // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<IEnumerable<Setor>>> Get()
        {
            try
            {
                var setores = await _service.GetAllSetoresAsync();
                return Ok(setores);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a lista de setores." });
            }
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca um setor específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do setor.</param>
        /// <returns>O objeto Setor solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Setor))]           // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<Setor>> Get(int id)
        {
            try
            {
                var setor = await _service.GetSetorByIdAsync(id);
                return Ok(setor);
            }
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
        /// <remarks>
        /// Exemplo de Payload (para PUT /api/Setor/5):
        ///
        ///     PUT /api/Setor/5
        ///     {
        ///        "nome": "Setor de aluguel"
        ///     }
        /// </remarks>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{idPassado}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta validações e CampoJaExistente
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Put(int idPassado, [FromBody] SetorDTO setorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateSetorAsync(idPassado, setorDto);
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
        /// <remarks>
        /// Exemplo de Payload:
        ///
        ///     POST /api/Setor
        ///     {
        ///        "nome": "BLOCO C"
        ///     }
        /// </remarks>
        /// <returns>O novo setor criado ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Setor))]     // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta validações e unicidade
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Post([FromBody] SetorDTO setorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var novoSetor = await _service.CreateSetorAsync(setorDto);

                return CreatedAtAction(nameof(Get), new { id = novoSetor.Id }, novoSetor);
            }
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
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