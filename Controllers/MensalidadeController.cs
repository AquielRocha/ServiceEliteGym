using Microsoft.AspNetCore.Mvc;
using SERVICE.Data;
using SERVICE.Models;
using Microsoft.EntityFrameworkCore;

namespace SERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensalidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MensalidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/mensalidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mensalidade>>> GetMensalidades()
        {
            return await _context.Mensalidades
                .Include(m => m.Aluno)
                .Include(m => m.Plano)
                .ToListAsync();
        }

        // GET: api/mensalidades/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensalidade>> GetMensalidade(int id)
        {
            var mensalidade = await _context.Mensalidades
                .Include(m => m.Aluno)
                .Include(m => m.Plano)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mensalidade == null)
            {
                return NotFound();
            }

            return mensalidade;
        }

        // POST: api/mensalidades/add
        [HttpPost("add")]
        public async Task<ActionResult<Mensalidade>> AddMensalidade([FromBody] Mensalidade mensalidade)
        {
            if (mensalidade == null)
            {
                return BadRequest("Dados da mensalidade inválidos.");
            }

            _context.Mensalidades.Add(mensalidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMensalidade), new { id = mensalidade.Id }, mensalidade);
        }

        // PUT: api/mensalidades/edit/{id}
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditMensalidade(int id, [FromBody] Mensalidade mensalidade)
        {
            if (id != mensalidade.Id)
            {
                return BadRequest();
            }

            _context.Entry(mensalidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensalidadeExists(id))
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

        // DELETE: api/mensalidades/del/{id}
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeleteMensalidade(int id)
        {
            var mensalidade = await _context.Mensalidades.FindAsync(id);
            if (mensalidade == null)
            {
                return NotFound();
            }

            _context.Mensalidades.Remove(mensalidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensalidadeExists(int id)
        {
            return _context.Mensalidades.Any(e => e.Id == id);
        }
    }
}
