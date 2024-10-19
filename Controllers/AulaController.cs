
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
            // Verifique se a aula já está sendo rastreada no contexto local
            var aulaExistente = _context.Aulas.Local.FirstOrDefault(a => a.Id == aula.Id);

            if (aulaExistente != null)
            {
                // Atualiza os dados da entidade rastreada em vez de adicionar uma nova
                _context.Entry(aulaExistente).CurrentValues.SetValues(aula);
            }
            else
            {
                // Se não estiver sendo rastreada, adicione a aula ao contexto
                _context.Aulas.Add(aula);
            }

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

            var aulaExistente = _context.Aulas.Local.FirstOrDefault(a => a.Id == id);
            
            // Se a aula já estiver sendo rastreada no contexto, sincronize as mudanças
            if (aulaExistente != null)
            {
                _context.Entry(aulaExistente).CurrentValues.SetValues(aula);  // Atualize os valores
            }
            else
            {
                _context.Entry(aula).State = EntityState.Modified;  // Atualize a aula diretamente
            }

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
        [HttpDelete("del/{id}")]
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

        [HttpPost("inscrever")]
        public async Task<ActionResult> InscreverAluno(int aulaId, int alunoId)
        {
            var aula = await _context.Aulas.Include(a => a.AlunosInscritos).FirstOrDefaultAsync(a => a.Id == aulaId);
            var aluno = await _context.Alunos.FindAsync(alunoId);

            if (aula == null || aluno == null)
            {
                return NotFound("Aula ou Aluno não encontrado.");
            }

            // Verifica se ainda há vagas
            if (aula.AlunosInscritos.Count >= aula.NumeroVagas)
            {
                return BadRequest("Não há vagas disponíveis para esta aula.");
            }

            // Verifica se o aluno já está inscrito
            if (aula.AlunosInscritos.Any(a => a.Id == alunoId))
            {
                return BadRequest("Aluno já está inscrito nesta aula.");
            }

            // Inscreve o aluno
            aula.AlunosInscritos.Add(aluno);
            aluno.Aulas.Add(aula);

            await _context.SaveChangesAsync();

            return Ok("Aluno inscrito com sucesso.");
        }

        // Método para listar as aulas de um aluno
        [HttpGet("aulasAluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<Aula>>> GetAulasDoAluno(int alunoId)
        {
            var aluno = await _context.Alunos
                .Include(a => a.Aulas)
                .FirstOrDefaultAsync(a => a.Id == alunoId);

            if (aluno == null)
            {
                return NotFound("Aluno não encontrado.");
            }

            return Ok(aluno.Aulas);
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