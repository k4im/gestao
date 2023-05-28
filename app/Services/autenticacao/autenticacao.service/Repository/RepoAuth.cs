namespace autenticacao.service.Repository
{
    public class RepoAuth : IRepoAuth
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IJwtHelper _jwtManager;

        public RepoAuth(UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager, 
        RoleManager<IdentityRole> roleManager, 
        IJwtHelper jwtManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtManager = jwtManager;
        }

        public Task<Response<AppUser>> listarUsuarios(int pagina, float resultado)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseLoginDTO> logar(LoginDTO loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.ChaveDeAcesso, loginModel.Senha, false, true);
            if(result.Succeeded)
            {
                var token = await _jwtManager.criarAccessToken(loginModel.ChaveDeAcesso);
                return new ResponseLoginDTO(token, "");
            }
            return new ResponseLoginDTO("Senha ou usuario invalidos!", "Senha ou usuario invalidos!");
        }

        public Task logOut()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseRegistroDTO> registrarUsuario(NovoUsuarioDTO user)
        {
            throw new NotImplementedException();
        }
    }
}