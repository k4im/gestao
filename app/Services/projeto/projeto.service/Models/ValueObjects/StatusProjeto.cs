
namespace projeto.service.Models.ValueObjects
{
    public class StatusProjeto
    {
        public string Status { get; private set; }

        protected StatusProjeto() { }
        public StatusProjeto(string status)
        {
            ValidarStatus(status);
            Status = status;
        }

        void ValidarStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) throw new Exception("O status não pode estar nulo!");
            if (!Regex.IsMatch(status, @"^[a-zA-ZzáàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]+$")) throw new Exception("o status não pode conter caracteres especiais");
        }

        public void AtualizacaoDoStatus(string novoStatus)
        {
            this.Status = novoStatus;
        }
    }
}
