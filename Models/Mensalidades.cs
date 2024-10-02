namespace SERVICE.Models{public class Mensalidade
{
    public int Id { get; set; }
    public int AlunoId { get; set; } // Renomeado para AlunoId
    public Alunos Aluno { get; set; } // Relaciona a mensalidade ao aluno
    public int PlanoId { get; set; }
    public Plano Plano { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string Status { get; set; } // Pendente, Pago, Vencido
}



}