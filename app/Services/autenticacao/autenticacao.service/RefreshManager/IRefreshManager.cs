namespace autenticacao.service.RefreshManager
{
    public interface IRefreshManager
    {
        Task<RefreshToken> BuscarRefreshToken(string username, string refreshToken);
        RefreshToken GerarRefreshToken(string ChaveDeAcesso);

        Task SalvarRefreshToken(RefreshToken request);

        Task DeletarRefreshToken(string username);
    }
}