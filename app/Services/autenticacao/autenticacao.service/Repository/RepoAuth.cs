namespace autenticacao.service.Repository
{
    public class RepoAuth : IRepoAuth
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IjwtManager _jwtManager;
        readonly IChaveManager _chaveManager;
        readonly IRefreshManager _refreshManager;
        readonly DataContext _db;

        public RepoAuth(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IjwtManager jwtManager, IChaveManager chaveManager, IRefreshManager refreshManager, DataContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtManager = jwtManager;
            _chaveManager = chaveManager;
            _refreshManager = refreshManager;
            _db = db;
        }

        public async Task<bool> desativarUsuario(string chave)
        {
            var usuario = await _userManager.FindByEmailAsync(chave);
            if (usuario == null) return false;
            usuario.desativarUsuario();
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<Response<AppUser>> listarUsuarios(int pagina, float resultado)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseLoginDTO> logar(LoginDTO loginModel)
        {
            var usuario = await _userManager.FindByNameAsync(loginModel.ChaveDeAcesso);
            if (!usuario.FlagDesativado)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.ChaveDeAcesso, loginModel.Senha, false, true);
                if (result.Succeeded)
                {
                    var token = await _jwtManager.criarAccessToken(loginModel.ChaveDeAcesso);
                    var RToken = _refreshManager.GerarRefreshToken(loginModel.ChaveDeAcesso);
                    await _refreshManager.SalvarRefreshToken(RToken);
                    return new ResponseLoginDTO(token, RToken.Token);
                }
            }
            return new ResponseLoginDTO("Senha ou usuario invalidos!", "Senha ou usuario invalidos!");
        }

        public Task logOut()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseRegistroDTO> registrarUsuario(NovoUsuarioDTO user)
        {
            var chave = await _chaveManager.gerarChaveDeAcesso();
            var NovoUsuario = new AppUser
            {
                UserName = chave,
                Email = chave,
                EmailConfirmed = true,
                Role = user.Papel

            };
            var result = await _userManager.CreateAsync(NovoUsuario, user.Senha);
            if (result.Succeeded)
            {
                await criarRoles();
                await _userManager.SetLockoutEnabledAsync(NovoUsuario, false);
                await _userManager.AddToRoleAsync(NovoUsuario, user.Papel);
            }
            if (!result.Succeeded && result.Errors.Count() > 0) Console.WriteLine("Erro");

            return new ResponseRegistroDTO(chave);
        }

        async Task criarRoles()
        {
            if (!_roleManager.RoleExistsAsync("ADMIN").GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole("ADMIN"));
                await _roleManager.CreateAsync(new IdentityRole("ATENDENTE"));
                await _roleManager.CreateAsync(new IdentityRole("FINANCEIRO"));
            }
        }
    }
}