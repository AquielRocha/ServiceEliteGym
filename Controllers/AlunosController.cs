using Microsoft.AspNetCore.Mvc;
using SERVICE.Data;
using SERVICE.Models;
using SERVICE.Dtos.Alunos;
using Microsoft.EntityFrameworkCore;

namespace SERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlunosController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/alunos
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Alunos>>> GetAllAlunos()
        {
            var alunos = await _context.Alunos
                .Include(a => a.EnderecosJoin) // Inclui os endereços associados
                .ToListAsync();

            return Ok(alunos);
        }

        // POST: api/alunos/add
        [HttpPost("add")]
        public async Task<ActionResult<Alunos>> PostAlunoComEnderecos([FromBody] CreateAlunoComEnderecosDto createAlunoComEnderecosDto)
        {
            // Cria o aluno
            var aluno = new Alunos
            {
                Nome = createAlunoComEnderecosDto.Nome,
                Email = createAlunoComEnderecosDto.Email,
                Foto = createAlunoComEnderecosDto.Foto,
                Tipo = createAlunoComEnderecosDto.Tipo,
                DataNascimento = createAlunoComEnderecosDto.DataNascimento,
                Telefone = createAlunoComEnderecosDto.Telefone,
                DataCadastro = DateTime.UtcNow,
                Objetivos = createAlunoComEnderecosDto.Objetivos,
                TipoPlano = createAlunoComEnderecosDto.TipoPlano,
                StatusPagamento = createAlunoComEnderecosDto.StatusPagamento,
                InformacoesMedicas = createAlunoComEnderecosDto.InformacoesMedicas,
                PreferenciasTreino = createAlunoComEnderecosDto.PreferenciasTreino,
                // Aulas = createAlunoComEnderecosDto.Aulas,
                Ativo = createAlunoComEnderecosDto.Ativo
            };

            // Adiciona o aluno ao contexto
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            // Se existirem endereços, cria-os e associa ao aluno
            if (createAlunoComEnderecosDto.Enderecos != null && createAlunoComEnderecosDto.Enderecos.Any())
            {
                foreach (var enderecoDto in createAlunoComEnderecosDto.Enderecos)
                {
                    var endereco = new Enderecos
                    {
                        Rua = enderecoDto.Rua,
                        Numero = enderecoDto.Numero,
                        Complemento = enderecoDto.Complemento,
                        Bairro = enderecoDto.Bairro,
                        Cidade = enderecoDto.Cidade,
                        Estado = enderecoDto.Estado,
                        CodigoPostal = enderecoDto.CodigoPostal,
                        Pais = enderecoDto.Pais,
                        DataCadastro = DateTime.UtcNow,
                        AlunoId = aluno.Id
                    };

                    _context.Enderecos.Add(endereco);
                }
                await _context.SaveChangesAsync();
            }

            // Retorna o aluno criado com os endereços associados
            return CreatedAtAction(nameof(GetAluno), new { id = aluno.Id }, aluno);
        }

        // GET: api/alunos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Alunos>> GetAluno(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return aluno;
        }


        // // PUT: api/alunos/edit/5
        // [HttpPut("edit/{id}")]
        // public async Task<IActionResult> PutAluno(int id, Alunos aluno)
        // {
        //     if (id != aluno.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(aluno).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!AlunoExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // DELETE: api/alunos/del/5
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }


    }
}
