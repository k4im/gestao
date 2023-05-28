namespace autenticacao.service.ChaveManager
{
    public interface IChaveManager
    {
        Task<string> gerarChaveDeAcesso();
    }
}