namespace SERVICE.Models
{
    public class Enderecos
    {
        public int Id { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public DateTime DataCadastro { get; set; }

        // Chave estrangeira
        public int AlunoId { get; set; }
        public Alunos Aluno { get; set; }
    }
}
