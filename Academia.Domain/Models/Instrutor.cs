namespace Academia.Domain.Models
{
    public class Instrutor
    {
        public int InstrutorId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Especialidade { get; set; }
        public DateTime DataContratacao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}

