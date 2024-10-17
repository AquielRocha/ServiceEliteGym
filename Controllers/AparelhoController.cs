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
                if (aparelho == null)
                {
                    return BadRequest("O aparelho não pode ser nulo.");
                }

                if (!string.IsNullOrEmpty(aparelho.Foto))
                {
                    if (!IsBase64String(aparelho.Foto))
                    {
                        return BadRequest("A imagem fornecida não é uma string Base64 válida.");
                    }
                }

                _context.Aparelhos.Add(aparelho);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAparelho), new { id = aparelho.Id }, aparelho);
            }
           catch (DbUpdateException dbEx)
           {
                Console.WriteLine($"Erro ao salvar no banco de dados: {dbEx.Message}");
                Console.WriteLine(dbEx.InnerException?.Message); // Mensagem de erro interna

                return StatusCode(500, "Erro ao salvar o aparelho no banco de dados.");
            }
            catch (Exception ex)
            {
                // Log detalhado para qualquer outra exceção
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro inesperado.");
            }
        }
        // Método auxiliar para verificar se a string é Base64 válida (opcional)
        private bool IsBase64String(string base64)
        {
            base64 = base64.Trim();
            return (base64.Length % 4 == 0) && System.Text.RegularExpressions.Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,2}$", System.Text.RegularExpressions.RegexOptions.None);
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
