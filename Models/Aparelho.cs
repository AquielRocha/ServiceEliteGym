namespace SERVICE.Models
{
    public class Aparelho
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Foto { get; set; }
        public string Categoria { get; set; }
        public bool Manutencao { get; set; }
        public bool Favorite { get; set; } 
    }
}
