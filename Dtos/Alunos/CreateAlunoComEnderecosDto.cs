using SERVICE.Dtos.Enderecos;

namespace SERVICE.Dtos.Alunos
{
 public class CreateAlunoComEnderecosDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Foto { get; set; }
    public string Tipo { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Telefone { get; set; }
    public string Objetivos { get; set; }
    public string TipoPlano { get; set; }
    public string StatusPagamento { get; set; }
    public string InformacoesMedicas { get; set; }
    public string PreferenciasTreino { get; set; }
    public bool Ativo { get; set; }
    public int PlanoId { get; set; } // Novo campo para associar um plano
    public List<EnderecoDto> Enderecos { get; set; } // Presumindo que você tenha um DTO para Endereços
}


}
