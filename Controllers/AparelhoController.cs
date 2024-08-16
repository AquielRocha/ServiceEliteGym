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
        public async Task<ActionResult<Aparelho>> PostAparelho(Aparelho aparelho)
        {
            if (aparelho == null)
            {
                return BadRequest("O aparelho não pode ser nulo.");
            }

            _context.Aparelhos.Add(aparelho);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAparelho), new { id = aparelho.Id }, aparelho);
        }

        /// <summary>
        /// Updates an existing appliance in the database.
        /// </summary>
        /// <param name="id">The ID of the appliance to update.</param>
        /// <param name="aparelho">The updated appliance details.</param>
        /// <returns>The updated appliance.</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutAparelho(int id, Aparelho aparelho)
        {
            if (id != aparelho.Id)
            {
                return BadRequest("O ID do aparelho não corresponde ao ID da URL.");
            }

            _context.Entry(aparelho).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
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
