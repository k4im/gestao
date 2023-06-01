namespace autenticacao.service.Controllers
{
    [ApiController]
    [Route("api/v0.1/[controller]")]
    public class PessoaControllers : ControllerBase
    {
        readonly IRepoPessoa _repo;

        public PessoaControllers(IRepoPessoa repo)
        {
            _repo = repo;
        }
        
        [HttpGet("pessoas/{pagina?}/{resultado?}"), AllowAnonymous]
        public async Task<IActionResult> buscarPessoas(int pagina = 1, float resultado = 5)
        {
            var pessoas =  await _repo.buscarPessoas(pagina, resultado);
            if(pessoas == null) return NotFound();
            return StatusCode(200, pessoas);
        }

        [HttpGet("pessoa/{id}"), 
        AllowAnonymous]
        public async Task<IActionResult> buscarPessoa(int id)
        {
            var pessoa = await _repo.buscarPessoaId(id);
            if(pessoa == null) return NotFound();
            return StatusCode(200, pessoa);
        }

        [HttpPost("pessoa/adicionar")]
        public async Task<IActionResult> adicionarPessoa(Pessoa model)
        {
            var result = await _repo.criarPessoa(model);
            return (result) ? StatusCode(200, "Pessoa adicionada com sucesso!"): 
            StatusCode(500, "Não foi possivel adicionar a pessoa!");
        }
        
        [HttpPut("pessoa/atualizar/endereco/{id}")]
        public async Task<IActionResult> atualizarPessoaEndereco(int id, Endereco model)
        {
            var result = await _repo.atualizarEndereco(id, model);
            if(result == null) return StatusCode(404);
            return StatusCode(200, result);
        }

        [HttpPut("pessoa/atualizar/telefone/{id}")]
        public async Task<IActionResult> atualizarTelefone(int id, Telefone model)
        {
            var result = await _repo.atualizarTelefone(id, model);
            if(result == null) return StatusCode(404);
            return StatusCode(200, result);
        }
    
    
        [HttpDelete("pessoa/deletar/{id}"),
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> deletarPessoa(int id)
        {
            var result = await _repo.deletarPessoa(id);
            return (result) ? StatusCode(200, "Pessoa removida com sucesso") : 
            StatusCode(500, "Não foi possivel remover a pessoa") ;
        }
    }
}