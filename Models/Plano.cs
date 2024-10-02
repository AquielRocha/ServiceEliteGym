public class Plano
{
    public int Id { get; set; }
    public string Nome { get; set; } // Ex: "Mensal", "Trimestral", "Anual"
    public decimal Valor { get; set; }
    public string Descricao { get; set; } // Informações adicionais sobre o plano
}
