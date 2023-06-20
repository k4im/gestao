
namespace projeto.service.Models.ValueObjects
{
    public class ChapaUtilizada
    {
        public string Chapa { get; private set; }

        protected ChapaUtilizada() { }
        public ChapaUtilizada(string chapa)
        {
            ValidarChapa(chapa);
            Chapa = chapa;
        }

        void ValidarChapa(string chapa)
        {
            if (string.IsNullOrEmpty(chapa)) throw new Exception("A chapa não pode estar nula!");
            if (!Regex.IsMatch(chapa, @"^[a-zA-Z ]+$")) throw new Exception("A rua não pode conter caracteres especiais");

        }


    }
}
