namespace autenticacao.service.RefreshManagers
{
    public class RefreshManager : IRefreshManager
    {
        readonly DataContext _db;
        readonly IMapper _mapper;
        readonly Logger.Logger _logger;
        public RefreshManager(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public RefreshManager(DataContext db, IMapper mapper, Logger.Logger logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RefreshToken> BuscarRefreshToken(string chave, string refreshToken)
        {
            var item = await _db.RTokens.FirstOrDefaultAsync(x => x.Usuario == chave);
            if (item == null) Results.NotFound("Não existe um refresh token para esse usuario");
            var RToken = new RefreshToken(item.RToken, item.Usuario, item.DataCriacao, item.DataExpiracao);
            return RToken;
        }

        public async Task<bool> DeletarRefreshToken(string token)
        {
            var refreshToken = await _db.RTokens.FirstOrDefaultAsync(x => x.RToken == token);
            if (refreshToken == null) Results.NotFound();
            try
            {
                _db.RTokens.Remove(refreshToken);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.logarErro($"Não foi possivel deletar o refresh Token: {e.Message}");
                return false;
            }
        }

        public RefreshToken GerarRefreshToken(string ChaveDeAcesso)
        {
            var random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 50).Select(x => pool[random.Next(0, pool.Length)]);
            var randomToken = new string(chars.ToArray());
            byte[] tokenBytes = Encoding.UTF8.GetBytes(randomToken.ToString());
            var tokenConverted = Convert.ToBase64String(tokenBytes);

            return new RefreshToken(tokenConverted, ChaveDeAcesso);
        }

        public async Task<bool> SalvarRefreshToken(RefreshToken request)
        {
            var tokenConvertido = new RefreshTokenTable
            {
                Usuario = request.Usuario,
                DataCriacao = request.DataDeCriacao,
                DataExpiracao = request.DataDeExpiracao,
                RToken = request.Token
            };
            try
            {
                _db.RTokens.Add(tokenConvertido);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.logarErro($"Não foi possivel salvar o refresh token: {e.Message}");
                return false;
            }
        }
    }
}