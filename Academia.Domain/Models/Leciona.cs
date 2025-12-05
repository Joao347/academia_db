namespace Academia.Domain.Models
{
    public class Leciona
    {
        public int InstrutorId { get; set; }
        public int MembroId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Observacao { get; set; }
        
        // Propriedades de navegação
        public Instrutor? Instrutor { get; set; }
        public Membro? Membro { get; set; }
    }
}

