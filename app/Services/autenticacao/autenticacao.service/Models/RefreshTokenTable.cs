namespace autenticacao.service.Models
{
    public class RefreshTokenTable
    {
        public int Id { get; set; }
        public string RToken { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataExpiracao { get; set; }
    }
}