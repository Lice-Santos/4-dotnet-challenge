using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Services;
using Tria_2025.Exceptions;
using Tria_2025.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO.MotoSetor;
using Microsoft.AspNetCore.Authorization;

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pela entidade de ligação MotoSetor.
    /// Delega a validação da existência de Moto e Setor ao MotoSetorService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MotoSetorController : ControllerBase
    {
        private readonly MotoSetorService _service;

        /// <summary>
        /// Construtor que injeta o serviço de MotoSetor.
        /// </summary>
        /// <param name="service">O serviço de MotoSetor para orquestração.</param>
        public MotoSetorController(MotoSetorService service)
        {
            _service = service;
        }

        // --- GET ALL ---
        /// <summary>
        /// Obtém a lista completa de todas as associações Moto-Setor.
        /// </summary>
        /// <returns>Uma lista de objetos MotoSetor.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MotoSetor>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                     // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<IEnumerable<MotoSetor>>> Get()
        {
            try
            {
                var associacoes = await _service.GetAllMotoSetoresAsync();
                return Ok(associacoes);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a lista de associações." });
            }
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca uma associação Moto-Setor pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da associação.</param>
        /// <returns>O objeto MotoSetor solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotoSetor))]         // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<MotoSetor>> Get(int id)
        {
            try
            {
                var motoSetor = await _service.GetMotoSetorByIdAsync(id);
                return Ok(motoSetor);
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar o registro." });
            }
        }

        // --- PUT (ATUALIZAÇÃO) ---
        /// <summary>
        /// Atualiza uma associação Moto-Setor, verificando a validade de IdMoto e IdSetor.
        /// </summary>
        /// <param name="id">O ID da associação a ser atualizada.</param>
        /// <param name="dto">O DTO com os novos dados.</param>
        /// <remarks>
        /// Exemplo de Payload (para PUT /api/MotoSetor/1):
        ///
        ///     PUT /api/MotoSetor/1
        ///     {
        ///        "data": "2025-10-01T14:30:00",
        ///        "fonte": "Ordem de Serviço 456 - Atualizada",
        ///        "idMoto": 101, // Moto permanece a mesma
        ///        "idSetor": 3   // Setor alterado
        ///     }
        /// </remarks>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta validações (ModelState, CampoInválido)
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Put(int id, [FromBody] MotoSetorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateMotoSetorAsync(id, dto);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (CampoInvalidoException ex)
            {
                // Usado para indicar que IdMoto ou IdSetor são inválidos/inexistentes no Service
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao atualizar o registro." });
            }
        }

        // --- POST (CRIAÇÃO) ---
        /// <summary>
        /// Cria uma nova associação Moto-Setor, validando a existência de IdMoto e IdSetor.
        /// </summary>
        /// <param name="motoSetorDto">Dados da associação.</param>
        /// <remarks>
        /// Exemplo de Payload:
        ///
        ///     POST /api/MotoSetor
        ///     {
        ///        "data": "2025-10-01T10:00:00",
        ///        "fonte": "Mudança inicial de setor",
        ///        "idMoto": 101,
        ///        "idSetor": 2
        ///     }
        /// </remarks>
        /// <returns>A nova associação criada ou 400 Bad Request em caso de falha.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MotoSetor))]    // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta validações e FKs inexistentes
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Post([FromBody] MotoSetorDTO motoSetorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var novaAssociacao = await _service.CreateMotoSetorAsync(motoSetorDto);

                return CreatedAtAction(nameof(Get), new { id = novaAssociacao.Id }, novaAssociacao);
            }
            // ObjetoNaoEncontradoException aqui significa que IdMoto ou IdSetor não existem
            catch (ObjetoNaoEncontradoException ex)
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
        /// Exclui uma associação Moto-Setor permanentemente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da associação a ser excluída.</param>
        /// <returns>204 No Content ou 404 Not Found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteMotoSetorAsync(id);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao deletar o registro." });
            }
        }
    }
}