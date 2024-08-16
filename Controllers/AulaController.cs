
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVICE.Data;
using SERVICE.Models;

namespace SERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AulasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AulasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/aulas
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Aula>>> GetAulas()
        {
            return await _context.Aulas.ToListAsync();
        }

        // GET: api/aulas/5
        [HttpGet("GET{id}")]
        public async Task<ActionResult<Aula>> GetAula(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);

            if (aula == null)
            {
                return NotFound();
            }

            return aula;
        }

        // POST: api/aulas
        [HttpPost("add")]
        public async Task<ActionResult<Aula>> PostAula(Aula aula)
        {
            _context.Aulas.Add(aula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAula), new { id = aula.Id }, aula);
        }

        // PUT: api/aulas/5
[HttpPut("edit/{id}")]
public async Task<IActionResult> PutAula(int id, Aula aula)
{
    if (id != aula.Id)
    {
        return BadRequest();
    }

    _context.Entry(aula).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!AulaExists(id))
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

        // DELETE: api/aulas/5
        [HttpDelete("del{id}")]
        public async Task<IActionResult> DeleteAula(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }

            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.Id == id);
        }
        
        [HttpGet("test-connection")]
public async Task<ActionResult<string>> TestConnection()
{
    try
    {
        var count = await _context.Aulas.CountAsync();
        return Ok($"Conexão bem-sucedida! Total de aulas: {count}");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Erro ao conectar ao banco de dados: {ex.Message}");
    }
}

    }
    
}