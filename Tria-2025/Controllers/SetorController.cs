using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.Models;

namespace Tria_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetorController : ControllerBase
    {
        public readonly AppDbContext _context;

        public SetorController(AppDbContext context)
        {
            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Setor>>> Get()
        {
            return await _context.Setores.ToListAsync();
        }

        //Buscar por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Setor>> Get(int id)
        {
            var setor = await _context.Setores.FindAsync(id);
            if (setor == null)
            {
                return NotFound();
            }

            return Ok(setor);
        }

        //PUT
        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, Setor setor)
        {
            if (setor.Id != idPassado)
            {
                return BadRequest("ID da url é diferente da url passada no corpo.");
            }

            var setorBuscado = await _context.Setores.FindAsync(idPassado);
            if (setorBuscado == null)
            {
                return NotFound($"Não foi possivel encontrar p setor com o id {idPassado}");
            }

            if (setor.Nome != null)
                setorBuscado.Nome = setor.Nome;


            await _context.SaveChangesAsync();
            return NoContent();
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Post(Setor setor)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(setor);
            }

            _context.Setores.Add(setor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = setor.Id }, setor);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var setor = await _context.Setores.FindAsync(id);
            if (setor == null) return NotFound($"Não fói possível encontrar o setor com o id {id}");

            _context.Setores.Remove(setor);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
