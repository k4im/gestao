namespace autenticacao.service.Models.UserController
{
    public class NovoUsuarioDTO
    {
        public NovoUsuarioDTO(string chaveDeAcesso, string senha, string papel)
        {
            ChaveDeAcesso = chaveDeAcesso;
            Senha = senha;
            Papel = papel;
        }

        public string ChaveDeAcesso { get;}
        public string Senha { get;}
        public string Papel { get;}
    }
}