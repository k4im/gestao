namespace autenticacao.service.Models.UserController
{
    public class ResponseLoginDTO
    {
        public ResponseLoginDTO(string accessToken, string refrehsToken)
        {
            AccessToken = validarToken(accessToken);
            RefrehsToken = validarToken(refrehsToken);
        }

        public string AccessToken { get;}
        public string RefrehsToken { get;}


        string validarToken(string token)
        {
            if(string.IsNullOrEmpty(token)) throw new Exception("Token não pode estar vazio!");
            return token;
        }
    }
}