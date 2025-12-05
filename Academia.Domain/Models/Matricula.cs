namespace Academia.Domain.Models
{
    public class Matricula
    {
        public int MatriculaId { get; set; }
        public int MembroId { get; set; }
        public int PlanoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal ValorPago { get; set; }
        public string Status { get; set; } = "Ativa";
        
        // Propriedades de navegação
        public Membro? Membro { get; set; }
        public Plano? Plano { get; set; }
    }
}



