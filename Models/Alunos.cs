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
        public string TipoPlano { get; set; }
        // public int HistoricoTreinos { get; set; }
        public string StatusPagamento { get; set; }
        public string InformacoesMedicas { get; set; }
        public string PreferenciasTreino { get; set; }

        public int aulas {get; set;}
        public bool Ativo { get; set; }
        public ICollection<Enderecos> EnderecosJoin { get; set; } = new List<Enderecos>();
    }
    
    // FUTURAMENTE para armazenar histórico de treinos
    // public class Treino
    // {
    //     public int Id { get; set; }
    //     public string Descricao { get; set; }
    //     public DateTime Data { get; set; }
    // }
}
