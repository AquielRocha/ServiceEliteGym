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
        public async Task<ActionResult<IEnumerable<ReadAlunoDto>>> GetAllAlunos()
        {
            var alunos = await _context.Alunos
                .Include(a => a.EnderecosJoin) // Inclui os endereços associados
                .Include(a => a.Mensalidades) // Inclui as mensalidades associadas
                .ThenInclude(m => m.Plano) // Inclui o plano associado
                .ToListAsync();

            // Cria uma lista de DTOs para retornar
            var alunosDto = alunos.Select(a => new ReadAlunoDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Email = a.Email,
                FotoBase64 = a.Foto,
                Tipo = a.Tipo,
                DataNascimento = a.DataNascimento,
                Telefone = a.Telefone,
                DataCadastro = a.DataCadastro,
                Objetivos = a.Objetivos,
              
                

                Ativo = a.Ativo,
                Enderecos = a.EnderecosJoin.Select(e => new EnderecoDto
                {
                    Rua = e.Rua,
                    Numero = e.Numero,
                    Complemento = e.Complemento,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    CodigoPostal = e.CodigoPostal,
                    Pais = e.Pais
                }).ToList(),
                Mensalidades = a.Mensalidades.Select(m => new MensalidadeDto
                {
                    Id = m.Id,
                    ValorMensalidade = m.Plano.Valor,
                    DataVencimento = m.DataVencimento,
                    DataPagamento = m.DataPagamento,
                    Status = m.Status
                }).ToList()
            });

            return Ok(alunosDto);
        }

        // POST: api/alunos/add
        [HttpPost("add")]
        public async Task<ActionResult<Alunos>> PostAlunoComEnderecos([FromBody] CreateAlunoComEnderecosDto createAlunoComEnderecosDto)
        {
            // Verifica se o plano existe
            var plano = await _context.Planos.FindAsync(createAlunoComEnderecosDto.PlanoId);
            if (plano == null)
            {
                return BadRequest("Plano não encontrado.");
            }

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
              
            
                Ativo = createAlunoComEnderecosDto.Ativo
            };

            // Adiciona o aluno ao contexto
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            // Criação da mensalidade associada ao aluno
            var mensalidade = new Mensalidade
            {
                AlunoId = aluno.Id,
                PlanoId = plano.Id,
                DataVencimento = DateTime.UtcNow.AddMonths(1), // Define a data de vencimento da mensalidade
                Status = "Pendente"
            };

            // Adiciona a mensalidade ao contexto
            _context.Mensalidades.Add(mensalidade);
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
        public async Task<ActionResult<ReadAlunoDto>> GetAluno(int id)
        {
            var aluno = await _context.Alunos
                .Include(a => a.EnderecosJoin) // Inclui os endereços associados
                .Include(a => a.Mensalidades) // Inclui as mensalidades associadas
                .ThenInclude(m => m.Plano) // Inclui o plano associado
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            // Cria o DTO para retornar
            var alunoDto = new ReadAlunoDto
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                FotoBase64 = aluno.Foto,
                Tipo = aluno.Tipo,
                DataNascimento = aluno.DataNascimento,
                Telefone = aluno.Telefone,
                DataCadastro = aluno.DataCadastro,
                Objetivos = aluno.Objetivos,
                
              
                Ativo = aluno.Ativo,
                Enderecos = aluno.EnderecosJoin.Select(e => new EnderecoDto
                {
                    Rua = e.Rua,
                    Numero = e.Numero,
                    Complemento = e.Complemento,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    CodigoPostal = e.CodigoPostal,
                    Pais = e.Pais
                }).ToList(),
                Mensalidades = aluno.Mensalidades.Select(m => new MensalidadeDto
                {
                    Id = m.Id,
                    ValorMensalidade = m.Plano.Valor,
                    DataVencimento = m.DataVencimento,
                    DataPagamento = m.DataPagamento,
                    Status = m.Status
                }).ToList()
            };

            return Ok(alunoDto);
        }

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
