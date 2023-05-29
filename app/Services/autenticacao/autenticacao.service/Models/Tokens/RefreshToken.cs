namespace autenticacao.service.Models.Tokens
{
    public class RefreshToken : TokenEntity
    {
        public RefreshToken(string token) : base(token)
        {}
        public RefreshToken(string token, string usuario) : base(token)
        {
            Usuario = usuario;
        }

        public RefreshToken(string token, string usuario, DateTime dataCriacao, DateTime dataExpiracao) : 
        base(token, dataCriacao, dataExpiracao)
        {
            Usuario = usuario;
            DataCriacao = dataCriacao;
            DataExpiracao = dataExpiracao;
        }

        public string Usuario { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataExpiracao { get; private set; }
    }
}