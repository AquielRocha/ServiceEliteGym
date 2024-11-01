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
        public DbSet<User> Users { get; set; }
        public DbSet<Aparelho> Aparelhos { get; set; }
        public DbSet<Alunos> Alunos { get; set; }
        public DbSet<Enderecos> Enderecos { get; set; }

  public DbSet<Mensalidade> Mensalidades { get; set; }
    public DbSet<Plano> Planos { get; set; } // Mapeamento do DbSet para Planos







            //Inner Joins 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alunos>()
                .HasMany(a => a.EnderecosJoin)
                .WithOne(e => e.Aluno)
                .HasForeignKey(e => e.AlunoId);

            // Configurações adicionais se necessárias
        }
    }
}
