using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVICE.Data;
using SERVICE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AparelhosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AparelhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all appliances.
        /// </summary>
        /// <returns>A list of all appliances.</returns>
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Aparelho>>> GetAllAparelhos()
        {
            var aparelhos = await _context.Aparelhos.ToListAsync();

            if (aparelhos == null || !aparelhos.Any())
            {
                return NotFound("Nenhum aparelho encontrado.");
            }

            return Ok(aparelhos);
        }

        /// <summary>
        /// Retrieves a specific appliance by ID.
        /// </summary>
        /// <param name="id">The ID of the appliance to retrieve.</param>
        /// <returns>The requested appliance.</returns>
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Aparelho>> GetAparelho(int id)
        {
            var aparelho = await _context.Aparelhos.FindAsync(id);

            if (aparelho == null)
            {
                return NotFound("Nenhum aparelho encontrado com o ID fornecido.");
            }

            return Ok(aparelho);
        }

        /// <summary>
        /// Adds a new appliance to the database.
        /// </summary>
        /// <param name="aparelho">The appliance to add.</param>
                /// <returns>The created appliance.</returns>
        [HttpPost("add")]
        public async Task<ActionResult<Aparelho>> PostAparelho([FromBody] Aparelho aparelho)
        {
            try
            {
                // Verifica se o objeto enviado é nulo
                if (aparelho == null)
                {
                    return BadRequest("O aparelho não pode ser nulo.");
                }

                // Verifica se a URL da foto fornecida é válida (opcional)
                if (!string.IsNullOrEmpty(aparelho.Foto) && !Uri.IsWellFormedUriString(aparelho.Foto, UriKind.Absolute))
                {
                    return BadRequest("A URL da imagem fornecida não é válida.");
                }

                // Adiciona o aparelho ao contexto
                _context.Aparelhos.Add(aparelho);
                await _context.SaveChangesAsync();

                // Retorna o recurso criado
                return CreatedAtAction(nameof(GetAparelho), new { id = aparelho.Id }, aparelho);
            }
            catch (Exception ex)
            {
                // Log detalhado para exceções
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro inesperado no servidor.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAparelho(int id, Aparelho aparelho)
        {
            if (id != aparelho.Id)
            {
                return BadRequest("ID do aparelho não corresponde.");
            }

            _context.Entry(aparelho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AparelhoExists(id))
                {
                    return NotFound("Aparelho não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AparelhoExists(int id)
        {
            return _context.Aparelhos.Any(e => e.Id == id);
        }


        /// <summary>
        /// Deletes a specific appliance by ID.
        /// </summary>
        /// <param name="id">The ID of the appliance to delete.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAparelho(int id)
        {
            var aparelho = await _context.Aparelhos.FindAsync(id);

            if (aparelho == null)
            {
                return NotFound("Nenhum aparelho encontrado com o ID fornecido.");
            }

            _context.Aparelhos.Remove(aparelho);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
