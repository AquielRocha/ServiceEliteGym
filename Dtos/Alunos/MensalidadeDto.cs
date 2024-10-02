public class MensalidadeDto
{
    public int Id { get; set; }
    public decimal ValorMensalidade { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string Status { get; set; }

    public PlanoDto Plano { get; set; }
}
