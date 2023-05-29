namespace autenticacao.service.RefreshManagers
{
    public interface IRefreshManager
    {
        Task<RefreshToken> BuscarRefreshToken(string chave, string refreshToken);
        RefreshToken GerarRefreshToken(string ChaveDeAcesso);

        Task<bool> SalvarRefreshToken(RefreshToken request);

        Task<bool> DeletarRefreshToken(string username);
    }
}