namespace autenticacao.service.chaveManager
{
    public interface IChaveManager
    {
        Task<string> gerarChaveDeAcesso();
    }
}