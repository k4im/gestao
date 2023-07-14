
using AutoMapper;
using estoque.service.AssynComm;

namespace estoque.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        readonly IRepoEstoque _repo;
        readonly GrayLogger _logger;

        public ProdutosController(IRepoEstoque repo, GrayLogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Este metodo estarpa realizando retornando uma lista contendo as pagina atual, total por pagina e uma lista contendo os produtos.
        /// </summary>
        /// <response code="200">Retorna a lista com os dados necessários</response>
        /// <response code="404">Informa que não foi possivel localizar a lista de produtos</response>
        [HttpGet("{pagina?}/{resultado?}")]
        // [Authorize(Roles = "ADMIN,ATENDENTE")]
        public async Task<IActionResult> buscarProdutos(int pagina = 1, int resultado = 5)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            // Recupera ID de correlacao
            // var teste = HttpContext.Request.Headers["X-Correlation-ID"].ToString();
            // Console.WriteLine($"TESTADA: [{teste}]");
            var produtos = await _repo.buscarProdutos(pagina, resultado);
            if (produtos == null)
            {
                _logger.logarAviso($"Não foi possivel buscar uma lista de produtos. Requirido por: [{currentUser}]");
                return StatusCode(404, "Não foi possivel identificar nenhum produto!");
            }
            _logger.logarInfo($"Retornado lista de produtos para: [{currentUser}]");
            return StatusCode(200, produtos);
        }

        /// <summary>
        ///  Estará realizando o a pesquisa de um produto a partir de um ID   
        /// </summary>
        /// <response code="200">Retorna o produto</response>
        /// <response code="404">Informa que não foi possivel estar encontrando o produto.</response>
        /// <response code="400">Retorna BadRequest e informa que é necessário ter um id para pequisa</response>
        [HttpGet("{id}")]
        // [Authorize(Roles = "ADMIN,ATENDENTE")]
        public async Task<IActionResult> buscarProdutoId(int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (id == 0 || id < 0) return StatusCode(404, "Por favor insira um id valido");
            if (id == null)
            {
                _logger.logarAviso($"ID INVALIDO: [{id}]. Requirido por [{currentUser}]");
                return StatusCode(400, "Por favor informe um Id");
            }
            var produto = await _repo.buscarProdutoId(id);
            if (produto == null)
            {
                _logger.logarAviso($"Não existe um produto com ID [{id}]. Ação feita por [{currentUser}]");
                return StatusCode(404, "Produto não encontrado!");
            }
            _logger.logarInfo($"Retornado produto para: [{currentUser}]");
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
        //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> adicionarProduto(Produto model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid)
            {
                _logger.logarErro($"Modelo invalido ao tentar adicionar um produto. Ação feita por [{currentUser}]");
            }
            var result = await _repo.adicionarProduto(model);
            if (result)
            {
                // _logger.logarInfo($"Adicionado produto com nome [{model.Nome}]. Ação realizada por [{currentUser}]");
                return StatusCode(201, "Produto adicionado com sucesso");
            }
            _logger.logarFatal($"Erro interno ao tentar adicionar produto. Ação feita por [{currentUser}]");
            return StatusCode(500, "Algo deu errado!");
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
        // [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> atualizarProduto(int? id, Produto model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (id == 0 || id < 0)
            {
                _logger.logarAviso($"ID INVALIDO: [{id}]. Requirido por [{currentUser}]");
                return StatusCode(404, "Por favor insira um id valido");
            }
            if (id == null)
            {
                _logger.logarAviso($"ID INVALIDO: [{id}]. Requirido por [{currentUser}]");
                return StatusCode(404, "Por favor insira um id para atualizar");
            }
            if (!ModelState.IsValid)
            {
                _logger.logarErro($"Modelo invalido ao tentar atualizar produto. Ação feita por [{currentUser}]");
                return StatusCode(400, ModelState);
            }
            var result = await _repo.atualizarProduto(id, model);
            if (result)
            {
                _logger.logarInfo($"Atualizado produto comID: [{id}]. Ação feita por [{currentUser}]");
                return StatusCode(200, "Produto atualizado com sucesso!");
            }
            return StatusCode(500, "Não foi possivel atualizar o produto!");
        }

        /// <summary>
        /// Realiza e remoção de um produto.
        /// </summary>
        /// <response code="404">Informa que não foi possivel localizar um produto com este ID</response>
        /// <response code="200">Informa que o produto foi deletado com sucesso</response>
        /// <response code="500">Informa que não foi possivel realizar a operação, erro do lado do servidor</response>
        [HttpDelete("produto_delete/{id?}")]
        // [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> deletarProduto(int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (id == 0 || id < 0)
            {
                _logger.logarAviso($"ID INVALIDO: [{id}]. Requirido por [{currentUser}]");
                return StatusCode(404, "Por favor insira um id valido");
            }
            if (id == null)
            {
                _logger.logarAviso($"ID INVALIDO: [{id}]. Requirido por [{currentUser}]");
                return StatusCode(404, "Por favor insira um id para atualizar");
            }
            var result = await _repo.removerProduto(id);
            if (result)
            {
                _logger.logarInfo($"Produto com ID [{id}] deletado com sucesso. Ação feita por [{currentUser}]");
                return StatusCode(200, "Produto deletado com sucesso!");
            }
            _logger.logarFatal($"Não foi possivel deletar o produto com ID [{id}]. Ação feita por [{currentUser}]");
            return StatusCode(500, "Não foi possivel deletar o produto!");
        }
    }

}
