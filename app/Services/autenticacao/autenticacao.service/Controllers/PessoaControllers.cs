namespace autenticacao.service.Controllers
{
    [ApiController]
    [Route("api/v0.1/[controller]")]
    [Authorize]
    public class PessoaControllers : ControllerBase
    {
        readonly IRepoPessoa _repo;

        public PessoaControllers(IRepoPessoa repo)
        {
            _repo = repo;
        }

        [HttpGet("pessoas/{pagina?}/{resultado?}"),
        ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> buscarPessoas(int pagina = 1, float resultado = 5)
        {
            var pessoas =  await _repo.buscarPessoas(pagina, resultado);
            if(pessoas == null) return NotFound();
            return Ok(pessoas);
        }

        [HttpGet("pessoa/{id}"), 
        ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> buscarPessoa(int id)
        {
            var pessoa = await _repo.buscarPessoaId(id);
            if(pessoa == null) return NotFound();
            return Ok(pessoa);
        }

        [HttpPost("pessoa/adicionar"), ValidateAntiForgeryToken,
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> adicionarPessoa(Pessoa model)
        {
            var result = await _repo.criarPessoa(model);
            return (result) ? Ok("Pessoa adicionada com sucesso!"): 
            StatusCode(500, "Não foi possivel adicionar a pessoa!");
        }
    
        [HttpPut("pessoa/atualizar/{id}"), 
        ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> atualizarPessoa(int id, Pessoa model)
        {
            var result = await _repo.atualiarPessoa(id, model);
            return (result) ? Ok("Dados pessoais atualizados com sucesso!"): 
            StatusCode(500, "Não foi possivel atualizar os dados!");
        }
    
        [HttpDelete("pessoa/deletar/{id}"),
        ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> deletarPessoa(int id)
        {
            var result = await _repo.deletarPessoa(id);
            return (result) ? Ok("Pessoa removida com sucesso") : 
            StatusCode(500, "Não foi possivel remover a pessoa") ;
        }
    }
}