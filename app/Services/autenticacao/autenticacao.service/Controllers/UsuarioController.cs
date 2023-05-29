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
        ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> buscarUsuarios(int pagina, float resultado)
        {
            var usuarios = await _repoAuth.listarUsuarios(pagina, resultado);
            if(usuarios == null) return NotFound("Não existe usuarios criados!");
            return Ok(usuarios);
        }

        [HttpPost("usuarios/novo"),
        ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> criarUsuario(NovoUsuarioDTO user)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _repoAuth.registrarUsuario(user);
                if(result == null) return StatusCode(500, "Algo deu errado!");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Algo deu errado!");
            }
        }

        [HttpPost("usuarios/desativar/{chave}"),
        ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> desativarUsuario(string chave)
        {
            var result = await _repoAuth.desativarUsuario(chave);
            return (result) ? Ok("Usuario desativado com sucesso!") 
            : StatusCode(500, "Não foi possivel desativar o usuario"); 
        }
    } 
}