namespace SERVICE.Models
{
    public class Alunos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }
        public string Tipo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Objetivos { get; set; }
        public bool Ativo { get; set; }

        // Relacionamento com aulas
        public ICollection<Aula> Aulas { get; set; } = new List<Aula>();
        
        public ICollection<Mensalidade> Mensalidades { get; set; } = new List<Mensalidade>();
        public ICollection<Enderecos> EnderecosJoin { get; set; } = new List<Enderecos>();
    }
}