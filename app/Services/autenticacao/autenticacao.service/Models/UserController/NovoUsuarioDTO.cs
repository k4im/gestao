namespace autenticacao.service.Models.UserController
{
    public class NovoUsuarioDTO
    {
        public NovoUsuarioDTO(string senha, string papel)
        {
            Senha = senha;
            Papel = papel;
        }

        public string Senha { get;}
        public string Papel { get;}
    }
}