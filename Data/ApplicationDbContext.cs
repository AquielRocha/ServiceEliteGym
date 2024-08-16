using Microsoft.EntityFrameworkCore;
using SERVICE.Models;

namespace SERVICE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Aparelho> Aparelhos { get; set; }
    }
}
