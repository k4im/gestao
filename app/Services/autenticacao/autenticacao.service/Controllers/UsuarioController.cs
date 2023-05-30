namespace autenticacao.service.Controllers
{
    [ApiController]
    [Route("api/v0.1/[controller]")]
    public class UsuarioController : ControllerBase
    {
        readonly IRepoAuth _repoAuth;
        readonly IRefreshManager _refreshManager;
        readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IRepoAuth repoAuth, IRefreshManager refreshManager, ILogger<UsuarioController> logger)
        {
            _repoAuth = repoAuth;
            _refreshManager = refreshManager;
            _logger = logger;
        }

        [HttpGet("usuarios/{pagina?}/{resultado?}"),
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> buscarUsuarios(int pagina = 1, float resultado = 5)
        {
            var usuarios = await _repoAuth.listarUsuarios(pagina, resultado);
            if(usuarios == null) return NotFound("Não existe usuarios criados!");
            return StatusCode(200, usuarios);
        }

        [HttpPost("usuarios/novo")]
        public async Task<IActionResult> criarUsuario(NovoUsuarioDTO user)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _repoAuth.registrarUsuario(user);
                if(result == null) return StatusCode(500, "Algo deu errado!");
                return StatusCode(200, result);
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Algo deu errado!");
            }
        }

        [HttpPost("usuarios/desativar/{chave}"),
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> desativarUsuario(string chave)
        {
            var result = await _repoAuth.desativarUsuario(chave);
            return (result) ? StatusCode(200, "Usuario desativado com sucesso!") 
            : StatusCode(500, "Não foi possivel desativar o usuario"); 
        }

        [HttpPost("usuarios/login")]
        public async Task<IActionResult> login(LoginDTO login)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _repoAuth.logar(login);
                if(result == null) return NotFound();
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Algo deu errado!");
            }
        }
        [HttpPost("usuarios/logout"),
        Authorize]
        public async Task<IActionResult> logOut()
        {
            await _repoAuth.logOut();
            return StatusCode(200);
        }
    } 
}