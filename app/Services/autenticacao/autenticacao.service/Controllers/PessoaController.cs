namespace autenticacao.service.Controllers
{
    public class PessoaController : ControllerBase
    {
        readonly IRepoPessoa _repo;

        public PessoaController(IRepoPessoa repo)
        {
            _repo = repo;
        }

        [HttpGet("pessoas/{pagina?}/{resultado?}")]
        public async Task<IActionResult> buscarPessoas(int pagina = 1, float resultado = 5)
        {
            var pessoas =  await _repo.buscarPessoas(pagina, resultado);
            return StatusCode(200, pessoas);
        }
    }
}