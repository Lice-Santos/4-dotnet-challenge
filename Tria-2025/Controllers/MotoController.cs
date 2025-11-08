using Microsoft.AspNetCore.Mvc;
using Tria_2025.Models;
using Tria_2025.Exceptions;
using Tria_2025.DTOs.Moto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Tria_2025.Services;
using Microsoft.AspNetCore.Authorization;

namespace Tria_2025.Controllers
{
    /// <summary>
    /// Controller responsável pelo CRUD e operações de Moto.
    /// Delega a lógica de negócio (validação de placa, ano, combustível e filial) ao MotoService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _service;

        /// <summary>
        /// Construtor que injeta o serviço de Moto.
        /// </summary>
        /// <param name="service">O serviço de Moto para orquestração.</param>
        public MotoController(MotoService service)
        {
            _service = service;
        }

        // --- GET ALL ---
        /// <summary>
        /// Obtém a lista completa de todas as motos cadastradas.
        /// </summary>
        /// <returns>Uma lista de objetos Moto.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Moto>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                 // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<IEnumerable<Moto>>> Get()
        {
            try
            {
                var motos = await _service.GetAllMotosAsync();
                return Ok(motos);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a lista de motos." });
            }
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca uma moto específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da moto.</param>
        /// <returns>O objeto Moto solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Moto))]           // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<Moto>> Get(int id)
        {
            try
            {
                var moto = await _service.GetMotoByIdAsync(id);
                return Ok(moto);
            }
            // Captura a exceção de Not Found
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a moto." });
            }
        }

        // --- GET POR ANO ---
        /// <summary>
        /// Busca todas as motos com o ano igual ou maior que o valor passado.
        /// </summary>
        /// <param name="ano">O ano mínimo para a pesquisa.</param>
        /// <returns>Lista de motos encontradas ou 404 Not Found.</returns>
        [HttpGet("ano/{ano}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Moto>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                             // ⭐️ Documenta não encontrado
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<List<Moto>>> BuscarMotosAcimaDoAno(int ano)
        {
            try
            {
                var motos = await _service.GetMotosAcimaDoAnoAsync(ano);

                if (motos == null || !motos.Any())
                {
                    return NotFound($"Nenhuma moto do ano {ano} ou superior foi encontrada.");
                }
                return Ok(motos);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar motos por ano." });
            }
        }

        // --- GET POR PLACA ---
        /// <summary>
        /// Busca uma moto específica pela placa.
        /// </summary>
        /// <param name="placa">A placa da moto.</param>
        /// <returns>O objeto Moto solicitado ou 404 Not Found.</returns>
        [HttpGet("placa/{placa}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Moto))]           // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<Moto>> BuscarPorPlaca(string placa)
        {
            try
            {
                // Busca a moto. Assume-se que o Service fará a validação da placa se necessário.
                var moto = await _service.GetMotoByPlacaAsync(placa);
                return Ok(moto);
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar a moto pela placa." });
            }
        }

        // --- GET POR MODELO ---
        /// <summary>
        /// Busca todas as motos com o modelo passado.
        /// </summary>
        /// <param name="modelo">O modelo da moto.</param>
        /// <returns>Lista de motos encontradas ou 404 Not Found.</returns>
        [HttpGet("modelo/{modelo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Moto>))] // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                             // ⭐️ Documenta não encontrado
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult<List<Moto>>> BuscarPorModelo(string modelo)
        {
            try
            {
                var motos = await _service.GetMotosByModeloAsync(modelo);

                if (motos == null || !motos.Any())
                {
                    return NotFound("Nenhuma moto com o modelo informado.");
                }
                return Ok(motos);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao buscar por modelo." });
            }
        }

        // --- PUT (ATUALIZAÇÃO) ---
        /// <summary>
        /// Atualiza as informações de uma moto existente.
        /// </summary>
        /// <param name="idPassado">O ID da moto a ser atualizada (da URL).</param>
        /// <param name="motoDTO">O objeto DTO com as novas informações.</param>
        /// <remarks>
        /// Exemplo de Payload (para PUT /api/Moto/1):
        ///
        ///     PUT /api/Moto/1
        ///     {
        ///        "placa": "ABC1234",
        ///        "modelo": "Honda CB 500F (Atualizada)",
        ///        "ano": 2024,
        ///        "tipoCombustivel": "Gasolina",
        ///        "idFilial": 2
        ///     }
        /// </remarks>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{idPassado}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta validações (Placa existente, inválido, etc.)
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Put(int idPassado, [FromBody] MotoDTO motoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service fará a validação de unicidade da placa e checagem da Filial
                await _service.UpdateMotoAsync(idPassado, motoDTO);
                return NoContent();
            }
            // Exceções de objeto não encontrado (404) ou FK (que o Service lança como 404)
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            // Exceções de validação (400) - Unicidade da Placa e Combustível/Ano Inválido
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
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao atualizar a moto." });
            }
        }

        // --- POST (CRIAÇÃO) ---
        /// <summary>
        /// Cria uma nova moto, verificando a unicidade da placa e a existência da Filial.
        /// </summary>
        /// <param name="motoDTO">Dados da Moto e IdFilial.</param>
        /// <remarks>
        /// Exemplo de Payload:
        ///
        ///     POST /api/Moto
        ///     {
        ///        "placa": "XYZ9876",
        ///        "modelo": "Honda CG Fan 160",
        ///        "ano": 2022,
        ///        "tipoCombustivel": "Flex",
        ///        "idFilial": 1
        ///     }
        /// </remarks>
        /// <returns>A nova moto criada ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Moto))]         // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                           // ⭐️ Documenta todas as exceções de validação
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                  // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Post([FromBody] MotoDTO motoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var novaMoto = await _service.CreateMotoAsync(motoDTO);

                return CreatedAtAction(nameof(Get), new { id = novaMoto.Id }, novaMoto);
            }
            catch (CampoJaExistenteException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            // 🛑 ObjetoNaoEncontradoException aqui geralmente significa que IdFilial não existe
            catch (ObjetoNaoEncontradoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (CampoInvalidoException ex)
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
        /// Exclui uma moto permanentemente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da moto a ser excluída.</param>
        /// <returns>204 No Content ou 404 Not Found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]                            // ⭐️ Documenta sucesso
        [ProducesResponseType(StatusCodes.Status404NotFound)]                           // ⭐️ Documenta ObjetoNaoEncontradoException
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]                // ⭐️ Documenta erro de servidor
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteMotoAsync(id);
                return NoContent();
            }
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno ao deletar a moto." });
            }
        }
    }
}