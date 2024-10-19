namespace SERVICE.Models
{
    public class Aula
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string? Foto { get; set; }
        public string? Video { get; set; }
        public string Tipo { get; set; }

        // Converte Data para UTC
        private DateTime _data;
        public DateTime Data
        {
            get => _data;
            set => _data = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public TimeSpan Horario { get; set; }
        public int NumeroVagas { get; set; }

        // Lista de alunos cadastrados na aula
        public ICollection<Alunos> AlunosInscritos { get; set; } = new List<Alunos>();
    }
}
