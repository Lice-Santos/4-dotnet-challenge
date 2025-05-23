using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.DTO;
using Tria_2025.Models;

namespace Tria_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilialController : ControllerBase
    {
        public readonly AppDbContext _context;

        public FilialController(AppDbContext context)
        {
            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filial>>> Get()
        {
            return await _context.Filiais.ToListAsync();
        }

        //Busca pelo id
        [HttpGet("{id}")]
        public async Task<ActionResult<Filial>> BuscarPorId(int id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null)
            {
                return NotFound();
            }

            return Ok(filial);
        }

        //Busca pelo nome da filial
        [HttpGet("nome/{nomeFilial}")]
        public async Task<ActionResult<List<Filial>>> BuscarPorNome(string nomeFilial)
        {
            var filiais = await _context.Filiais.Where(c => c.Nome.ToLower().Contains(nomeFilial.ToLower())).ToListAsync();
            if (filiais.Count == 0)
            {
                return NotFound();
            }

            return Ok(filiais);
        }

        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, FilialDTO filialDTO)
        {
            var filialBuscada = await _context.Filiais.FindAsync(idPassado);
            if (filialBuscada == null)
            {
                return NotFound($"Não foi possível encontrar uma filial com o ID {idPassado}.");
            }

            var endereco = await _context.Enderecos.FindAsync(filialDTO.IdEndereco);
            if (endereco == null)
            {
                return BadRequest("Endereço inválido. Não foi possível encontrar o endereço informado.");
            }

            filialBuscada.Nome = filialDTO.Nome;
            filialBuscada.IdEndereco = filialDTO.IdEndereco;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult> Post(FilialDTO filialDTO)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
        }
            var enderecoObjeto = await _context.Enderecos.FindAsync(filialDTO.IdEndereco);
            if (enderecoObjeto == null)
            {
                return BadRequest("Não foi possível encontrar o endereço passado");
            }

            var filialCompleta = new Filial
            {
                Nome = filialDTO.Nome,
                IdEndereco = filialDTO.IdEndereco,
                Endereco = enderecoObjeto
            };
            _context.Filiais.Add(filialCompleta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = filialCompleta.Id }, filialCompleta);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null) return NotFound($"Não fói possível encontrar uma filial com o id {id}");

            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
