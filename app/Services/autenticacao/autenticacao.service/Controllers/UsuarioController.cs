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
            if (usuarios == null) return NotFound("Não existe usuarios criados!");
            return StatusCode(200, usuarios);
        }
        /// <summary>
        /// Cria novo usuario.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     
        ///     {
        ///       "senha": "Sua Senha",
        ///       "papel": "ADMIN ou ATENDENTE"
        ///     }
        /// </remarks>
        /// <response code="200">Retorna a pessoa com o dado atualizado</response>
        /// <response code="500">Retorna que algo deu errado</response>
        [HttpPost("usuarios/novo")]
        public async Task<IActionResult> criarUsuario(NovoUsuarioDTO user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _repoAuth.registrarUsuario(user);
                if (result == null) return StatusCode(500, "Algo deu errado!");
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Algo deu errado!");
            }
        }

        [HttpPost("usuarios/desativar/{chave}"),
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> desativarUsuario([FromRoute] string chave)
        {
            var result = await _repoAuth.desativarUsuario(chave);
            return (result) ? StatusCode(200, "Usuario desativado com sucesso!")
            : StatusCode(500, "Não foi possivel desativar o usuario");
        }
        /// <summary>
        /// Realizar login.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     
        ///     {
        ///       "chaveDeAcesso": "Sua chave de acesso",
        ///       "senha": "Sua senha"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna a pessoa com o dado atualizado</response>
        /// <response code="500">Retorna que algo deu errado</response>
        [HttpPost("usuarios/login")]
        public async Task<IActionResult> login(LoginDTO login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _repoAuth.logar(login);
                if (result == null) return NotFound();
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
            return StatusCode(200, "Você realizou logout");
        }


        [HttpPost("usuarios/reativar/{chave}"),
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> reavitarUsuario([FromRoute] string chave)
        {
            var result = await _repoAuth.reativarUsuario(chave);
            return (result) ? StatusCode(200, "Usuario reativado com sucesso!")
            : StatusCode(500, "Não foi possivel desativar o usuario");
        }
    }
}