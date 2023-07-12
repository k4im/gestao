namespace projeto.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        readonly IRepoProjetos _repo;
        public ProjetoController(IRepoProjetos repo)
        {
            _repo = repo;
        }


        /// <summary>
        /// Estará realizando a paginação e retornando uma lista completamente paginada de todos os projetos no banco de dados
        /// </summary>
        /// <response code="200">Retorna a lista com todos os projetos paginados</response>
        [HttpGet("projetos/{pagina?}/{resultadoPorPagina?}")]
        [Authorize(Roles = "ADMIN,ATENDENTE")]
        public async Task<IActionResult> GetAllProjects(int pagina = 1, float resultadoPorPagina = 5)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var projetos = await _repo.BuscarProdutos(pagina, resultadoPorPagina);
            if (projetos == null)
            {
                return StatusCode(404);
            }
            return Ok(projetos);
        }

        /// <summary>
        /// Estará realizando a operação de busca de um projeto a partir de um ID
        /// </summary>
        /// <response code="200"> Retorna o projeto</response>
        /// <response code="404"> Não existe um projeto com este ID</response>
        [HttpGet("projeto/{id?}")]
        [Authorize(Roles = "ADMIN,ATENDENTE")]
        public async Task<IActionResult> GetById(int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var item = await _repo.BuscarPorId(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        /// <summary>
        /// Ira adicionar um novo projeto ao banco de dados, também estará realizando o envio do projeto para o service BUS
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///       "nome": "Cozinha",
        ///       "status": {
        ///         "status": "Em produção"
        ///       },
        ///       "dataInicio": "2023-07-12T17:11:22.745Z",
        ///       "dataEntrega": "2023-07-12T17:11:22.745Z",
        ///       "produtoUtilizado": 1, # ID do produto existente em estoque
        ///       "quantidadeUtilizado": 23,
        ///       "descricao": "Cozinha feita para fulano",
        ///       "valor": 3330
        ///     }
        ///
        /// </remarks>
        /// <response code="201"> Informa que tudo ocorreu como esperado</response>
        [HttpPost("Create")]
        // [Authorize(Roles = "ADMIN,ATENDENTE")]
        public async Task<IActionResult> adicionarProjeto(Projeto model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }
            try
            {
                var result = await _repo.CriarProjeto(model);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Estará realizando a atualização exclusivamente do status do projeto
        /// </summary>
        /// <response code="200"> Informa que tudo ocorreu como esperado</response>
        /// <response code="409"> Informa que houve um erro de conflito</response>
        /// <response code="404"> Informa que não foi possivel encontrar um produto com este ID</response>
        [HttpPut("update/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> atualizarStatusProjeto(StatusProjeto model, int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                await _repo.AtualizarStatus(model, id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(409, "Não foi possivel atualizar o item, o mesmo foi atualizado por outro usuario!");
            }
        }

        /// <summary>
        /// Estará realizando a operação de remoção do projeto do banco de dados
        /// </summary>
        /// <response code="204"> Retorna No content caso o projeto tenha sido deletado corretamente</response>
        /// <response code="409"> Informa que houve um erro de conflito</response>
        /// <response code="404"> Informa que não foi possivel encontrar um produto com este ID</response>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> removerProjeto(int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                await _repo.DeletarProjeto(id);
                return StatusCode(204);
            }
            catch (Exception)
            {
                return StatusCode(409, "Não foi possivel deletar o item, o mesmo foi deletado por outro usuario!");
            }
        }

    }
}