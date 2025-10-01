using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Services;
using Tria_2025.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tria_2025.DTO; // Usando o DTO de Filial que definimos

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pelo CRUD da entidade Filial.
    /// Delega a lógica de negócio, validação de unicidade e integridade referencial ao FilialService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FilialController : ControllerBase
    {
        private readonly FilialService _service;

        /// <summary>
        /// Construtor que injeta o serviço de Filial.
        /// </summary>
        /// <param name="service">O serviço de Filial para orquestração.</param>
        public FilialController(FilialService service)
        {
            _service = service;
        }

        // --- GET ALL ---
        /// <summary>
        /// Obtém a lista completa de todas as filiais cadastradas.
        /// </summary>
        /// <returns>Uma lista de objetos Filial.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filial>>> Get()
        {
            var filiais = await _service.GetAllFiliaisAsync();
            return Ok(filiais);
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca uma filial específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da filial.</param>
        /// <returns>O objeto Filial solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Filial>> BuscarPorId(int id)
        {
            try
            {
                var filial = await _service.GetFilialByIdAsync(id);
                return Ok(filial);
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a filial." });
            }
        }

        // --- GET POR NOME ---
        /// <summary>
        /// Busca filiais que contenham o nome passado na URL.
        /// </summary>
        /// <param name="nomeFilial">O nome ou parte do nome da filial.</param>
        /// <returns>Lista de filiais encontradas ou 404 Not Found.</returns>
        [HttpGet("nome/{nomeFilial}")]
        public async Task<ActionResult<List<Filial>>> BuscarPorNome(string nomeFilial)
        {
            // Note: Este método não usa Try-Catch para NotFound, pois retorna 404 se a lista estiver vazia.
            var filiais = await _service.GetFiliaisByNomeAsync(nomeFilial);

            if (filiais == null || !filiais.Any())
            {
                return NotFound("Nenhuma filial encontrada com o nome especificado.");
            }

            return Ok(filiais);
        }

        // --- POST (CRIAÇÃO) ---
        /// <summary>
        /// Cria uma nova filial, verificando a unicidade do nome e a validade do IdEndereco.
        /// </summary>
        /// <param name="filialDTO">Dados da Filial e IdEndereco.</param>
        /// <returns>A nova filial criada ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FilialDTO filialDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação de Unicidade e a checagem do IdEndereco
                var novaFilial = await _service.CreateFilialAsync(filialDTO);

                // Retorna 201 Created
                return CreatedAtAction(nameof(BuscarPorId), new { id = novaFilial.Id }, novaFilial);
            }
            // Captura as exceções de validação de regra de negócio (400 Bad Request)
            catch (CampoJaExistenteException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                // Usado aqui para a chave estrangeira IdEndereco não encontrada.
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

        // --- PUT (ATUALIZAÇÃO) ---
        /// <summary>
        /// Atualiza as informações de uma filial existente.
        /// </summary>
        /// <param name="idPassado">O ID da filial a ser atualizada (da URL).</param>
        /// <param name="filialDTO">O objeto DTO com as novas informações.</param>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, [FromBody] FilialDTO filialDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateFilialAsync(idPassado, filialDTO);
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
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao atualizar a filial." });
            }
        }

        // --- DELETE ---
        /// <summary>
        /// Exclui uma filial permanentemente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da filial a ser excluída.</param>
        /// <returns>204 No Content ou 404 Not Found.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteFilialAsync(id);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao deletar a filial." });
            }
        }
    }
}
