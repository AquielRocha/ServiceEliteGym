using SERVICE.Dtos.Enderecos;

namespace SERVICE.Dtos.Alunos
{
    public class ReadAlunoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string FotoBase64 { get; set; }
        public string Tipo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Objetivos { get; set; }
        public string TipoPlano { get; set; }
        public string StatusPagamento { get; set; }
        public string InformacoesMedicas { get; set; }
        public string PreferenciasTreino { get; set; }
        public bool Ativo { get; set; }
        public List<EnderecoDto> Enderecos { get; set; } // Lista de endereços

        // Adicionando a lista de mensalidades
        public List<MensalidadeDto> Mensalidades { get; set; } // Lista de mensalidades do aluno
    }
}
