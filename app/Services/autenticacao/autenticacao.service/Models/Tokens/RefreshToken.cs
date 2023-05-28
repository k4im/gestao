namespace autenticacao.service.Models.Tokens
{
    public class RefreshToken : TokenEntity
    {
        public RefreshToken(string token, string usuario) : base(token)
        {
            Usuario = usuario;
        }

        [Key]
        public int Id { get; private set; }

        public string Usuario { get; private set; }
    }
}