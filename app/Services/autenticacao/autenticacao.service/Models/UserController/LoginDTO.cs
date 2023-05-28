namespace autenticacao.service.Models.UserController
{
    public class LoginDTO
    {
        public LoginDTO(string chaveDeAcesso, string senha)
        {
            ChaveDeAcesso = chaveDeAcesso;
            Senha = senha;
        }

        public string ChaveDeAcesso { get;}  
        public string Senha { get;}  
    }
}