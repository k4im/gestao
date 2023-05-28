namespace autenticacao.service.jwtManager
{
    public interface IjwtManager
    {
        Task<string> criarAccessToken(string chaveDeAcesso);
        List<Claim> gerarClaims(AppUser user);
    }
}