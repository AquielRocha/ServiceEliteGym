
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVICE.Data;
using SERVICE.Models;

[ApiController]
[Route("api/[controller]")]
public class MensalidadeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MensalidadeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarMensalidade([FromBody] Mensalidade mensalidade)
    {
        _context.Mensalidades.Add(mensalidade);
        await _context.SaveChangesAsync();
        return Ok(mensalidade);
    }

    [HttpPut("{id}/postergar")]
    public async Task<IActionResult> PostergarVencimento(int id, [FromBody] int dias)
    {
        var mensalidade = await _context.Mensalidades.FindAsync(id);
        if (mensalidade == null)
            return NotFound();

        mensalidade.DataVencimento = mensalidade.DataVencimento.AddDays(dias);
        await _context.SaveChangesAsync();

        return Ok(mensalidade);
    }
}
