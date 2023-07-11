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
        public IActionResult mostrarProdutos()
        {
            var produtos = _repo.buscarTodosProdutos();
            return Ok(produtos);
        }
    }
}