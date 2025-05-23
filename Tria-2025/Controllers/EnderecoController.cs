using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Controllers
{
    //CRUD Endereco
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        public readonly AppDbContext _context;

        public EnderecoController(AppDbContext context)
        {
            _context = context;
        }

        //GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Endereco>>> Get()
        {
            return await _context.Enderecos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> BuscarPorId(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null)
            { 
                return NotFound();
            }

                return Ok(endereco);
        }

        //pesquisa pelo cep do endereço

        [HttpGet("cep/{cep}")]
        public async Task<ActionResult<Endereco>> BuscarPorCep(string cep)
        {
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(c => c.Cep == cep);
            if (endereco == null)
            {
                return NotFound();
            }

            return Ok(endereco);
        }

        //pesquisa todos os endereços que possuam o logradouro igual ao passado

        [HttpGet("logradouro/{logradouro}")]
        public async Task<ActionResult<List<Endereco>>> BuscarPorLogradouro(string logradouro)
        {
            var enderecos = await _context.Enderecos.Where(c => c.Logradouro.ToLower().Contains(logradouro.ToLower())).ToListAsync();
            if (enderecos.Count == 0)
            {
                return NotFound();
            }

            return Ok(enderecos);
        }

        //PUT

        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, Endereco endereco)
        {
            if (endereco.Id != idPassado)
            {
                return BadRequest("ID da url é diferente da url passada no corpo.");
            }

            var enderecoBuscado = await _context.Enderecos.FindAsync(idPassado);
            if (enderecoBuscado == null)
            {
                return NotFound($"Não foi possivel encontrar um endereço com o id {idPassado}");
            }

            if (endereco.Logradouro != null)
                enderecoBuscado.Logradouro = endereco.Logradouro;

            if (endereco.Cidade != null)
                enderecoBuscado.Cidade = endereco.Cidade;

            if (endereco.Estado != null)
                enderecoBuscado.Estado = endereco.Estado;

            if (endereco.Numero != null)
                enderecoBuscado.Numero = endereco.Numero;

            if (endereco.Complemento != null)
                enderecoBuscado.Complemento = endereco.Complemento;

            if (endereco.Cep != null)
                enderecoBuscado.Cep = endereco.Cep;


            await _context.SaveChangesAsync();
            return NoContent();
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Post(Endereco endereco)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(endereco);
            }

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = endereco.Id }, endereco);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null) return NotFound($"Não fói possível encontrar um endereço com o id {id}");

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
