namespace autenticacao.service.Models.UserController
{
    public class ResponseRegistroDTO
    {
        public ResponseRegistroDTO(string chaveDeAcesso)
        {
            ChaveDeAcesso = validarChave(chaveDeAcesso);
        }

        public string ChaveDeAcesso { get;}

        string validarChave(string chave)
        {
            if(string.IsNullOrEmpty(chave)) throw new Exception("Chave não pode estar vazia");
            return chave;
        }
    }
}