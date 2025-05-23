using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tria_2025.Connection;
using Tria_2025.DTO;
using Tria_2025.Models;

namespace Tria_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        public readonly AppDbContext _context;

        public MotoController (AppDbContext context)
        {
            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> Get()
        {
            return await _context.Motos.ToListAsync();
        }

        //GET por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> Get(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
            {
                return NotFound();
            }

            return Ok(moto);
        }

        //Busca todas as motos que possuam o ano igual ou maior que o passado
        [HttpGet("ano/{ano}")]
        public async Task<ActionResult<List<Moto>>> BuscarMotosAcimaDoAno(int ano)
        {
            var motos = await _context.Motos.Where(m => (m.Ano >= ano)).ToListAsync();
            if (motos.Count == 0)
            {
                return NotFound($"Nenhuma moto do ano {ano} ou superior foi encontrada.");
            }
            return Ok(motos);
        }

        //Busca uma moto pela placa
        [HttpGet("placa/{placa}")]
        public async Task<ActionResult<Moto>> BuscarPorPlaca(string placa)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
            if (moto == null)
            {
                return NotFound("Nenhuma moto registrada com a placa informada.");
            }

            return Ok(moto);
        }

        //Busca todas as motos com o modelo passado
        [HttpGet("modelo/{modelo}")]
        public async Task<ActionResult<List<Moto>>> BuscarPorModelo(string modelo)
        {
            var motos = await _context.Motos.Where(m => m.Modelo.ToLower() == modelo.ToLower()).ToListAsync();
            if (motos.Count == 0)
            {
                return NotFound("Nenhuma moto com o modelo informado.");
            }

            return Ok(motos);
        }

        //PUT
        [HttpPut("{idPassado}")]
        public async Task<ActionResult> Put(int idPassado, MotoDTO motoDTO)
        {
            var motoBuscada = await _context.Motos.FindAsync(idPassado);
            if (motoBuscada == null)
            {
                return NotFound($"Não foi possível encontrar uma moto com o id {idPassado}");
            }

            //Buscando com o id passado se a filial existe
            var filial = await _context.Filiais.FindAsync(motoDTO.IdFilial);
            if (filial == null)
            {
                return BadRequest("Não foi possível encontrar a filial informada.");
            }

            motoBuscada.Placa = motoDTO.Placa;
            motoBuscada.Modelo = motoDTO.Modelo;
            motoBuscada.TipoCombustivel = motoDTO.TipoCombustivel;
            motoBuscada.Ano = motoDTO.Ano;
            motoBuscada.IdFilial = motoDTO.IdFilial;
            motoBuscada.Filial = filial;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> Post(MotoDTO motoDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filialObjeto = await _context.Filiais.FindAsync(motoDTO.IdFilial);
            if (filialObjeto == null)
            {
                return BadRequest("Não foi possível encontrar a filial passada");
            }

            var motoCompleta = new Moto
            {
                Ano = motoDTO.Ano,
                Filial = filialObjeto,
                Placa = motoDTO.Placa,
                Modelo = motoDTO.Modelo,
                TipoCombustivel = motoDTO.TipoCombustivel
            };


            _context.Motos.Add(motoCompleta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = motoCompleta.Id }, motoCompleta);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound($"Não fói possível encontrar uma moto com o id {id}");

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
