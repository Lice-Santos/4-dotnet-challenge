using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Services;
using Tria_2025.Exceptions;
using Tria_2025.DTO; // Usando o DTO de MotoSetor
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO.MotoSetor;

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pela entidade de ligação MotoSetor.
    /// Delega a validação da existência de Moto e Setor ao MotoSetorService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<MotoSetor>>> Get()
        {
            var associacoes = await _service.GetAllMotoSetoresAsync();
            return Ok(associacoes);
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca uma associação Moto-Setor pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da associação.</param>
        /// <returns>O objeto MotoSetor solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
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
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MotoSetorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação das chaves estrangeiras
                await _service.UpdateMotoSetorAsync(id, dto);
                return NoContent();
            }
            // 404: Se o registro original (pelo ID da URL) não for encontrado
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            // 400: Se IdMoto ou IdSetor não existirem (lançado como BadRequest pelo Service)
            catch (CampoInvalidoException ex)
            {
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
        /// <returns>A nova associação criada ou 400 Bad Request em caso de falha.</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MotoSetorDTO motoSetorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação de existência de Moto e Setor.
                var novaAssociacao = await _service.CreateMotoSetorAsync(motoSetorDto);

                // Retorna 201 Created
                return CreatedAtAction(nameof(Get), new { id = novaAssociacao.Id }, novaAssociacao);
            }
            // 400: Captura erro se IdMoto ou IdSetor não existirem
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
