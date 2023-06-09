namespace estoque.service.Models.ValueObject
{
    public class NickName
    {
        public NickName(string nomeProjeto)
        {
            NomeProjeto = validarNickName(nomeProjeto);
        }

        public string NomeProjeto { get; private set; }

        string validarNickName(string nomeProjeto)
        {
            if (string.IsNullOrEmpty(nomeProjeto)) throw new Exception("Campo não deve ser nulo");
            if (!Regex.IsMatch(nomeProjeto, @"^[a-zA-Z]+$")) throw new Exception("O cep não pode conter caracteres especiais");
            return nomeProjeto;
        }
    }
}