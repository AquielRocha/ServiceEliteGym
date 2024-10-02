namespace SERVICE.Models
{public class Mensalidade
{
    public int Id { get; set; }

    // Relacionamento com Alunos
    public int AlunoId { get; set; }
    public Alunos Aluno { get; set; }

    // Relacionamento com Plano
    public int PlanoId { get; set; }
    public Plano Plano { get; set; }

    public decimal ValorMensalidade { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string Status { get; set; } // Pendente, Pago, Vencido
}

}