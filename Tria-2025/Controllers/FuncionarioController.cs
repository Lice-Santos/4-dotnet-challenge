using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        public readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> Get()
        {
            return await _context.Funcionarios.ToListAsync();
        }

        //GET por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> Get(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return Ok(funcionario);
        }

        // GET pelo nome do funcionário

        [HttpGet("nome/{nomeFuncionario}")]
        public async Task<ActionResult<List<Funcionario>>> BuscarFuncionarioPorNome(string nomeFuncionario)
        {
            var funcionarios = await _context.Funcionarios.Where(c => c.Nome.ToLower().Contains(nomeFuncionario.ToLower())).ToListAsync();
            if (funcionarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(funcionarios);
        }

        //Simula um login
        [HttpGet("login")]
        public async Task<ActionResult<string>> BuscarDadosParaLogin(string email, string senha)
        {
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower() && c.Senha.ToLower() == senha.ToLower());
            if (funcionario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            return Ok($"Seja bem vindo {funcionario.Nome}");
        }

        //Busca todos os funcionários com o cargo passado
        [HttpGet("cargo/{cargo}")]
        public async Task<ActionResult<List<Funcionario>>> BuscarPorCargo(string cargo)
        {
            var funcionarios = await _context.Funcionarios.Where(c => c.Cargo.ToLower().Contains(cargo.ToLower())).ToListAsync();
            if (funcionarios.Count == 0)
            {
                return NotFound("Nenhum funcionário encontrado");
            }

            return Ok(funcionarios);
        }

        //PUT
        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, Funcionario funcionario)
        {
            if (funcionario.Id != idPassado)
            {
                return BadRequest("ID da url é diferente da url passada no corpo.");
            }

            var funcionarioBuscado = await _context.Funcionarios.FindAsync(idPassado);
            if (funcionarioBuscado == null)
            {
                return NotFound($"Não foi possivel encontrar um funcionário com o id {idPassado}");
            }

            if (funcionario.Nome != null)
                funcionarioBuscado.Nome = funcionario.Nome;

            if (funcionario.Cargo != null)
                funcionarioBuscado.Cargo = funcionario.Cargo;

            if (funcionario.Email != null)
                funcionarioBuscado.Email = funcionario.Email;

            if (funcionario.Senha != null)
                funcionarioBuscado.Senha = funcionario.Senha;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Post(Funcionario funcionario)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(funcionario);
            }

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = funcionario.Id }, funcionario);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filial = await _context.Funcionarios.FindAsync(id);
            if (filial == null) return NotFound($"Não fói possível encontrar um funcionário com o id {id}");

            _context.Funcionarios.Remove(filial);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
