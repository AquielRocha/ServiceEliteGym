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

        // GET: api/mensalidades/aluno/{alunoId}
        [HttpGet("aluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<Mensalidade>>> GetMensalidadesByAluno(int alunoId)
        {
            var mensalidades = await _context.Mensalidades
                .Where(m => m.AlunoId == alunoId)
                .Include(m => m.Plano) // Inclui detalhes do plano, se necessário
                .ToListAsync();

            if (mensalidades == null || !mensalidades.Any())
            {
                return NotFound("Nenhuma mensalidade encontrada para o aluno.");
            }

            return Ok(mensalidades);
        }

        // PUT: api/mensalidades/edit/{id}
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateMensalidade(int id, Mensalidade mensalidade)
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

        private bool MensalidadeExists(int id)
        {
            return _context.Mensalidades.Any(e => e.Id == id);
        }
    }
}
