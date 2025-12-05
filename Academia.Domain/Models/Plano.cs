namespace Academia.Domain.Models
{
    public class Plano
    {
        public int PlanoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int DuracaoMeses { get; set; }
        public bool Ativo { get; set; } = true;
    }
}



