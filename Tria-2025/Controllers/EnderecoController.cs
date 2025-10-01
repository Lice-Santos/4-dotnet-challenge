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
    /// Controller responsável pelo CRUD da entidade Endereco.
    /// Utiliza EnderecoService para isolar a lógica de negócio e validação.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly EnderecoService _service;

        /// <summary>
        /// Construtor que injeta o serviço de Endereço.
        /// </summary>
        /// <param name="service">O serviço de Endereço para orquestração.</param>
        public EnderecoController(EnderecoService service)
        {
            _service = service;
        }

        // --- GET ALL ---
        /// <summary>
        /// Obtém a lista completa de todos os endereços cadastrados.
        /// </summary>
        /// <returns>Uma lista de objetos Endereco.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Endereco>>> Get()
        {
            // O Service retorna uma lista vazia se não houver registros.
            var enderecos = await _service.GetAllEnderecosAsync();
            return Ok(enderecos);
        }

        // --- GET POR ID ---
        /// <summary>
        /// Busca um endereço específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do endereço.</param>
        /// <returns>O objeto Endereco solicitado ou 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> BuscarPorId(int id)
        {
            try
            {
                var endereco = await _service.GetEnderecoByIdAsync(id);
                return Ok(endereco);
            }
            // Exceção de objeto não encontrado (404)
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor ao buscar o endereço." });
            }
        }

        // --- GET POR CEP ---
        /// <summary>
        /// Pesquisa um endereço pelo número de CEP.
        /// </summary>
        /// <param name="cep">O CEP a ser pesquisado (apenas dígitos).</param>
        /// <returns>O objeto Endereco correspondente ou 404/400.</returns>
        [HttpGet("cep/{cep}")]
        public async Task<ActionResult<Endereco>> BuscarPorCep(string cep)
        {
            try
            {
                // A validação de formato do CEP é feita dentro do Service antes da busca.
                var endereco = await _service.GetEnderecoByCepAsync(cep);
                return Ok(endereco);
            }
            // Exceção de formato inválido (400 Bad Request)
            catch (CepForaFormatacao ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            // Exceção de objeto não encontrado (404)
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor ao buscar o CEP." });
            }
        }

        // --- POST (CRIAÇÃO) ---
        /// <summary>
        /// Cria um novo endereço no banco de dados.
        /// </summary>
        /// <param name="endereco">O objeto Endereco a ser criado.</param>
        /// <returns>O novo endereço criado com o ID ou 400 Bad Request em caso de falha na validação.</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EnderecoDTO enderecoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 1. Cria a entidade a partir do DTO (sem Id)
                var endereco = new Endereco
                {
                    Logradouro = enderecoDto.Logradouro,
                    Cidade = enderecoDto.Cidade,
                    Estado = enderecoDto.Estado,
                    Numero = enderecoDto.Numero,
                    Complemento = enderecoDto.Complemento,
                    Cep = enderecoDto.Cep
                };

                // 2. Chama o Service, que valida e salva
                var novoEndereco = await _service.CreateEnderecoAsync(endereco);

                // 3. Retorna 201 Created com o recurso criado
                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = novoEndereco.Id },
                    novoEndereco
                );
            }
            catch (CampoVazioException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (TamanhoInvalidoDeCaracteresException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (CepForaFormatacao ex)
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
        /// Atualiza todas as informações de um endereço existente.
        /// </summary>
        /// <param name="id">O ID do endereço a ser atualizado (da URL).</param>
        /// <param name="endereco">O objeto Endereco com as novas informações.</param>
        /// <returns>204 No Content, 404 Not Found ou 400 Bad Request.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Endereco endereco)
        {
            // Validação básica do Controller
            if (endereco.Id != id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no corpo da requisição.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // O Service valida a existência do ID e o conteúdo do objeto.
                await _service.UpdateEnderecoAsync(id, endereco);
                return NoContent();
            }
            // Exceção de objeto não encontrado (404)
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            // Exceções de validação (400)
            catch (CampoVazioException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor ao atualizar o endereço." });
            }
        }

        // --- DELETE ---
        /// <summary>
        /// Exclui um endereço permanentemente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do endereço a ser excluído.</param>
        /// <returns>204 No Content ou 404 Not Found.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteEnderecoAsync(id);
                return NoContent();
            }
            // Exceção de objeto não encontrado (404)
            catch (ObjetoNaoEncontradoException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor ao deletar o endereço." });
            }
        }
    }
}
