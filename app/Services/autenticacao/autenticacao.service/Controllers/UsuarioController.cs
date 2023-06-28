namespace autenticacao.service.Controllers
{
    [ApiController]
    [Route("api/v0.1/[controller]")]
    public class UsuarioController : ControllerBase
    {
        readonly IRepoAuth _repoAuth;
        readonly IRefreshManager _refreshManager;
        readonly Logger.Logger _logger;

        public UsuarioController(IRepoAuth repoAuth, IRefreshManager refreshManager, Logger.Logger logger = null)
        {
            _repoAuth = repoAuth;
            _refreshManager = refreshManager;
            _logger = logger;
        }

        /// <summary>
        /// Retorna a lista de usuarios de forma paginada.
        /// </summary>
        /// <remarks>
        /// **Caso não seja adicionado um valor de pagina ou resultado, será atribuido como padrão o valor para pagina de 1 e o valor para o resultado por pagina de 5.**
        /// </remarks>
        /// <returns code="200">Retorna codigo 200 com a lista de usuarios</returns>
        /// <returns code="404">Informa que não existe uma lista de usuario</returns>
        [HttpGet("usuarios/{pagina?}/{resultado?}")]
        public async Task<IActionResult> buscarUsuarios(int pagina = 1, float resultado = 5)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var usuarios = await _repoAuth.listarUsuarios(pagina, resultado);
            if (usuarios == null)
            {
                _logger.logarAviso($"Não existem usuarios criados. Requirido por: [{currentUser}]");
                return StatusCode(404);
            }
            _logger.logarInfo($"Retornado lista de usuarios. Ação feita por: [{currentUser}]");
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
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid)
            {
                _logger.logarErro($"Não foi possivel criar o usuario, [Modelo Invalido]. Requirido por: [{currentUser}]");
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _repoAuth.registrarUsuario(user);
                if (result == null)
                {
                    _logger.logarAviso($"Não foi possivel criar o usuario. Requirido por: [{currentUser}]");
                    return StatusCode(500, "Algo deu errado!");
                }
                _logger.logarInfo($"Realizado criação do usuario, chave: [{result.ChaveDeAcesso}]. Ação feita por: [{currentUser}]");
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                _logger.logarErro(e.Message);
                return StatusCode(500, "Algo deu errado!");
            }
        }

        /// <summary>
        /// Ira realizar a operação de desativar um usuario
        /// </summary>
        /// <remarks>
        /// Caso o usuario seja desativado o mesmo não conseguira realizar mais login. É possivel estar reativando o usuario, porém enquanto o status permanecer o mesmo não tera mais acesso.
        /// </remarks>
        /// <returns code="200">Informa que foi possivel estar desativando o usuario</returns>
        /// <returns code="500">Informa que não foi possivel realizar a operação</returns>
        [HttpPost("usuarios/desativar/{chave}")]
        public async Task<IActionResult> desativarUsuario([FromRoute] string chave)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var result = await _repoAuth.desativarUsuario(chave);
            if (result)
            {
                _logger.logarInfo($"Realizado desativação do usuario: [{chave}]. Ação feita por: [{currentUser}]");
                return StatusCode(200, "Usuario desativado com sucesso!");
            }
            _logger.logarAviso($"Não foi possivel desativar o usuario: [{chave}]");
            return StatusCode(500, "Não foi possivel desativar o usuario");
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
                if (result == null)
                {
                    _logger.logarAviso($"Não foi encontado o usuario");
                    return NotFound();
                }
                _logger.logarInfo($"Realizado o login do usuario: {login.ChaveDeAcesso}");
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                _logger.logarErro($"Não foi possivel logar o usuario: [{login.ChaveDeAcesso}] - {e.Message}");
                return StatusCode(500, "Algo deu errado!");
            }
        }

        /// <summary>
        /// Ira realizar o LogOut do usuario que está atualmente conectado
        /// </summary>
        /// <returns code="200">Informa que o usuario conseguiu se desconectar</returns>
        [HttpPost("usuarios/logout"),
        Authorize]
        public async Task<IActionResult> logOut()
        {
            await _repoAuth.logOut();
            return StatusCode(200, "Você realizou logout");
        }

        /// <summary>
        /// Ira realizar a operação de reativar um usuario
        /// </summary>
        /// <returns code="200">Informa que foi possivel estar desativando o usuario</returns>
        /// <returns code="500">Informa que não foi possivel realizar a operação</returns>
        [HttpPost("usuarios/reativar/{chave}")]
        public async Task<IActionResult> reavitarUsuario([FromRoute] string chave)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var result = await _repoAuth.reativarUsuario(chave);
            if (result)
            {
                _logger.logarInfo($"Realizado reativação do usuario: [{chave}]. Ação feita por: [{currentUser}]");
                return StatusCode(200, "Usuario reativado com sucesso!");
            }
            return StatusCode(500, "Não foi possivel desativar o usuario");
        }
    }
}