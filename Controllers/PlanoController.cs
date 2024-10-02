using Microsoft.AspNetCore.Mvc;
using SERVICE.Data;
using SERVICE.Models;
using Microsoft.EntityFrameworkCore;

namespace SERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/planos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plano>>> GetPlanos()
        {
            return await _context.Planos.ToListAsync();
        }

        // GET: api/planos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Plano>> GetPlano(int id)
        {
            var plano = await _context.Planos.FindAsync(id);

            if (plano == null)
            {
                return NotFound();
            }

            return plano;
        }

        // POST: api/planos/add
        [HttpPost("add")]
        public async Task<ActionResult<Plano>> AddPlano([FromBody] Plano plano)
        {
            if (plano == null)
            {
                return BadRequest("Dados do plano inválidos.");
            }

            _context.Planos.Add(plano);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlano), new { id = plano.Id }, plano);
        }

        // PUT: api/planos/edit/{id}
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditPlano(int id, [FromBody] Plano plano)
        {
            if (id != plano.Id)
            {
                return BadRequest();
            }

            _context.Entry(plano).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/planos/del/{id}
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePlano(int id)
        {
            var plano = await _context.Planos.FindAsync(id);
            if (plano == null)
            {
                return NotFound();
            }

            _context.Planos.Remove(plano);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanoExists(int id)
        {
            return _context.Planos.Any(e => e.Id == id);
        }
    }
}
