namespace estoque.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        readonly IRepoEstoque _repo;

        public ProdutosController(IRepoEstoque repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Este metodo estarpa realizando retornando uma lista contendo as pagina atual, total por pagina e uma lista contendo os produtos.
        /// </summary>
        /// <response code="200">Retorna a lista com os dados necessários</response>
        /// <response code="404">Informa que não foi possivel localizar a lista de produtos</response>
        [HttpGet("{pagina?}/{resultado?}")]
        public async Task<IActionResult> buscarProdutos(int pagina = 1, int resultado = 5)
        {
            var produtos = await _repo.buscarProdutos(pagina, resultado);
            if (produtos == null) return StatusCode(404, "Não foi possivel identificar nenhum produto!");
            return StatusCode(200, produtos);
        }

        /// <summary>
        ///  Estará realizando o a pesquisa de um produto a partir de um ID   
        /// </summary>
        /// <response code="200">Retorna o produto</response>
        /// <response code="404">Informa que não foi possivel estar encontrando o produto.</response>
        /// <response code="400">Retorna BadRequest e informa que é necessário ter um id para pequisa</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> buscarProdutoId(int? id)
        {
            if (id == 0 || id < 0) return StatusCode(404, "Por favor insira um id valido");
            if (id == null) return StatusCode(400, "Por favor informe um Id");
            var produto = await _repo.buscarProdutoId(id);
            if (produto == null) return StatusCode(404, "Produto não encontrado!");
            return StatusCode(200, produto);
        }

        /// <summary>
        /// Irá realizar a adição de um produto no banco de dados.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///       "nome": "Chapa MDF Branca",
        ///       "valor": 150.00,
        ///       "quantidade": 3
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Informa que o produto foi criado com sucesso!</response>
        /// <response code="400">BadRequest, informa o campo que está errado no modelo</response>
        /// <response code="500">Informa que algo deu errado do lado do servidor</response>
        [HttpPost("novo_produto")]
        public async Task<IActionResult> adicionarProduto(Produto model)
        {
            if (!ModelState.IsValid) return StatusCode(400, ModelState);
            var result = await _repo.adicionarProduto(model);
            return result ? StatusCode(201, "Produto adicionado com sucesso") : StatusCode(500, "Algo deu errado!");
        }

        /// <summary>
        /// Irá realizar update de um produto.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///       "nome": "Chapa MDF Verde",
        ///       "valor": 150.00,
        ///       "quantidade": 3
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Informa que o produto foi atualizado com sucesso!</response>
        /// <response code="400">BadRequest, informa o campo que está errado no modelo</response>
        /// <response code="500">Informa que algo deu errado do lado do servidor</response>
        [HttpPut("produto_atualizar/{id?}")]
        public async Task<IActionResult> atualizarProduto(int? id, Produto model)
        {
            if (id == 0 || id < 0) return StatusCode(404, "Por favor insira um id valido");
            if (id == null) return StatusCode(404, "Por favor insira um id para atualizar");
            if (!ModelState.IsValid) return StatusCode(400, ModelState);
            var result = await _repo.atualizarProduto(id, model);
            return (result) ? StatusCode(200, "Produto atualizado com sucesso!") : StatusCode(500, "Não foi possivel atualizar o produto!");
        }

        /// <summary>
        /// Realiza e remoção de um produto.
        /// </summary>
        /// <response code="404">Informa que não foi possivel localizar um produto com este ID</response>
        /// <response code="200">Informa que o produto foi deletado com sucesso</response>
        /// <response code="500">Informa que não foi possivel realizar a operação, erro do lado do servidor</response>
        [HttpDelete("produto_delete/{id?}")]
        public async Task<IActionResult> deletarProduto(int? id)
        {
            if (id == 0 || id < 0) return StatusCode(404, "Por favor insira um id valido");
            if (id == null) return StatusCode(404, "Por favor insira um id");
            var result = await _repo.removerProduto(id);
            return (result) ? StatusCode(200, "Produto deletado com sucesso!") : StatusCode(500, "Não foi possivel deletar o produto!");
        }
    }

}