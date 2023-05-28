namespace autenticacao.service.JwtHelper
{
    public interface IJwtHelper
    {
        Task<string> criarAccessToken(string chaveDeAcesso);
        List<Claim> gerarClaims(AppUser user);
    }
}