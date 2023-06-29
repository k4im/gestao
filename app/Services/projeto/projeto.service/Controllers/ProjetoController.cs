namespace projeto.service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        readonly IRepoProjetos _repo;
        readonly IMessageBusService _msgBus;
        readonly IMapper _mapper;
        public ProjetoController(IRepoProjetos repo, IMessageBusService msg, IMapper mapper)
        {
            _repo = repo;
            _msgBus = msg;
            _mapper = mapper;
        }

        /// <summary>
        /// Estará realizando a paginação e retornando uma lista completamente paginada de todos os projetos no banco de dados
        /// </summary>
        /// <response code="200">Retorna a lista com todos os projetos paginados</response>
        [HttpGet("projetos/{pagina?}/{resultadoPorPagina?}")]
        [Authorize(Roles ="ADMIN, ATENDENTE")]
        public async Task<IActionResult> GetAllProjects(int pagina = 1, float resultadoPorPagina = 5)
        {
            var projetos = await _repo.BuscarProdutos(pagina, resultadoPorPagina);
            return Ok(projetos);
        }

        /// <summary>
        /// Estará realizando a operação de busca de um projeto a partir de um ID
        /// </summary>
        /// <response code="200"> Retorna o projeto</response>
        /// <response code="404"> Não existe um projeto com este ID</response>
        [HttpGet("projeto/{id?}")]
        [Authorize(Roles ="ADMIN, ATENDENTE")]
        public async Task<IActionResult> GetById(int? id)
        {
            var item = await _repo.BuscarPorId(id);
            if (item == null) return NotFound();
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
        [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> CreateProject(Projeto model)
        {

            if (ModelState.IsValid)
            {
                await _repo.CriarProjeto(model);
                try
                {
                    var projeto = _mapper.Map<Projeto, ProjetoDTO>(model);
                    _msgBus.publishNewProjeto(projeto);
                    Console.WriteLine("--> Mensagem enviado para o bus");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Não foi possivel enviar a mensagem para o bus: {e.Message}");
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id == null) return NotFound();
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
        [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> DeleteProject(int? id)
        {
            if (id == null) return NotFound();
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