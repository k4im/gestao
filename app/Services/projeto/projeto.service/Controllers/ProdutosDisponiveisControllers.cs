namespace projeto.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosDisponiveisControllers : ControllerBase
    {
        readonly IRepoProdutosDisponiveis _repo;

        public ProdutosDisponiveisControllers(IRepoProdutosDisponiveis repo)
        {
            _repo = repo;
        }

        [HttpGet("produtos_em_estoque")]
        public async Task<ActionResult<List<ProdutosDisponiveis>>> mostrarProdutos()
        {
            var produtos = await _repo.buscarTodosProdutos();
            return Ok(produtos);
        }
    }
}