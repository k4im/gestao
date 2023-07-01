namespace projeto.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        readonly IRepoProjetos _repo;
        readonly IMessageBusService _msgBus;
        readonly IMapper _mapper;
        readonly GrayLogger _logger;
        public ProjetoController(IRepoProjetos repo, IMessageBusService msg, IMapper mapper, GrayLogger logger)
        {
            _repo = repo;
            _msgBus = msg;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Estará realizando a paginação e retornando uma lista completamente paginada de todos os projetos no banco de dados
        /// </summary>
        /// <response code="200">Retorna a lista com todos os projetos paginados</response>
        [HttpGet("projetos/{pagina?}/{resultadoPorPagina?}")]
        [Authorize(Roles ="ADMIN,ATENDENTE")]
        public async Task<IActionResult> GetAllProjects(int pagina = 1, float resultadoPorPagina = 5)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var projetos = await _repo.BuscarProdutos(pagina, resultadoPorPagina);
            if (projetos == null) 
            {
                _logger.logarAviso($"Não foi possivel identificar a lista de projetos. Ação feita por [{currentUser}]");
                return StatusCode(404);
            }
            _logger.logarAviso($"Retornado lista de projetos. Ação feita por [{currentUser}]");
            return Ok(projetos);
        }

        /// <summary>
        /// Estará realizando a operação de busca de um projeto a partir de um ID
        /// </summary>
        /// <response code="200"> Retorna o projeto</response>
        /// <response code="404"> Não existe um projeto com este ID</response>
        [HttpGet("projeto/{id?}")]
        [Authorize(Roles ="ADMIN,ATENDENTE")]
        public async Task<IActionResult> GetById(int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var item = await _repo.BuscarPorId(id);
            if (item == null) 
            {
                _logger.logarAviso($"Não foi possivel identificar o projeto pelo ID: [{id}]. Ação feita por: [{currentUser}]");
                return NotFound();
            }
            _logger.logarInfo($"Retornado projeto com ID: [{id}]. Ação feita por [{currentUser}]");
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
        ///       "dataInicio": "2023-06-21T15:29:24.256Z",
        ///       "dataEntrega": "2023-08-25T15:29:24.256Z",
        ///       "chapa": {
        ///         "chapa": "Chapa Branca"
        ///       },
        ///       "descricao": "Projeto feito para o Ronaldo",
        ///       "quantidadeDeChapa": 5, # Chapas utilizadas
        ///       "valor": 3500.00
        ///     }
        ///
        /// </remarks>
        /// <response code="201"> Informa que tudo ocorreu como esperado</response>
        [HttpPost("Create")]
        [Authorize(Roles ="ADMIN,ATENDENTE")]
        public async Task<IActionResult> CreateProject(Projeto model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if(!ModelState.IsValid) 
            {
                _logger.logarAviso($"Tentativa de criar projeto invalido. Ação feita por [{currentUser}]");
                return StatusCode(400, ModelState);
            }
            if (ModelState.IsValid)
            {
                await _repo.CriarProjeto(model);
                _logger.logarInfo($"Adicionado projeto ao banco de dados com nome: [{model.Nome}]. Ação feita por: [{currentUser}]");
                try
                {
                    var projeto = _mapper.Map<Projeto, ProjetoDTO>(model);
                    _msgBus.publishNewProjeto(projeto);
                    _logger.logarInfo($"Realizado envio de mensagem para o RabbitMQ. Ação feita por [{currentUser}]");
                }
                catch (Exception e)
                {
                    _logger.logarErro($"--> Não foi possivel enviar a mensagem para o RabbitMQ: {e.Message}. Ação feita por [{currentUser}]");
                }
            }
            return StatusCode(201);
        }

        /// <summary>
        /// Estará realizando a atualização exclusivamente do status do projeto
        /// </summary>
        /// <response code="200"> Informa que tudo ocorreu como esperado</response>
        /// <response code="409"> Informa que houve um erro de conflito</response>
        /// <response code="404"> Informa que não foi possivel encontrar um produto com este ID</response>
        [HttpPut("update/{id}")]
        [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> UpdateProject(StatusProjeto model, int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id == null) 
            {
                _logger.logarAviso($"Não foi possivel atualizar o projeto com ID [{id}]. Ação feita por [{currentUser}]");
                return NotFound();
            }
            try
            {
                await _repo.AtualizarStatus(model, id);
                _logger.logarInfo($"Realizado atualização do projeto com ID [{id}]. Ação realizada por [{currentUser}]");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.logarErro($"Não foi possivel realizar a operação de atualização do projeto com ID [{id}]. Ação feita por [{currentUser}]. [ERRO] {e.Message}");
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
        [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> DeleteProject(int? id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (id == null) 
            {
                _logger.logarAviso($"Não foi possivel deletar o projeto com ID [{id}]. Ação feita por [{currentUser}]");
                return NotFound();
            }
            try
            {
                await _repo.DeletarProjeto(id);
                _logger.logarInfo($"Realizado remoção do projeto com ID [{id}]. Ação realizada por [{currentUser}]");
                return StatusCode(204);
            }
            catch (Exception e)
            {
                _logger.logarErro($"Não foi possivel realizar a operação de remoção do projeto com ID [{id}]. Ação feita por [{currentUser}]. [ERRO] {e.Message}");
                return StatusCode(409, "Não foi possivel deletar o item, o mesmo foi deletado por outro usuario!");
            }
        }

    }
}