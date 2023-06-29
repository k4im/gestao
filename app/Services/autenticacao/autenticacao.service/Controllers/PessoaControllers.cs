namespace autenticacao.service.Controllers
{
    [ApiController]
    [Route("api/v0.1/[controller]")]
    public class PessoaControllers : ControllerBase
    {
        readonly IRepoPessoa _repo;
        readonly Logger.Logger _logger;


        public PessoaControllers(IRepoPessoa repo, Logger.Logger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Retorna uma lista paginada de Pessoas.
        /// </summary>
        [HttpGet("pessoas/{pagina?}/{resultado?}")]
        [Authorize(Roles="ADMIN, ATENDENTE")]
        public async Task<IActionResult> buscarPessoas(int pagina = 1, float resultado = 5)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var pessoas = await _repo.buscarPessoas(pagina, resultado);
            if (pessoas == null)
            {
                _logger.logarAviso($"Não foi possivel identificar uma lista de pessoas. Requirido por: [{currentUser}]");
                return StatusCode(404);
            }
            _logger.logarInfo($"Retornado lista de pessoas. Ação feita por: [{currentUser}]");
            return StatusCode(200, pessoas);
        }

        /// <summary>
        /// Ira retornar uma pessoa a partir de um ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns code="200">Retorna a pessoa pertencente ao id</returns>
        /// <returns code="404">Informa que não existe uma pessoa com tal ID</returns>
        [HttpGet("pessoa/{id}")]
        [Authorize(Roles="ADMIN, ATENDENTE")]
        public async Task<IActionResult> buscarPessoa(int id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var pessoa = await _repo.buscarPessoaId(id);
            if (pessoa == null)
            {
                _logger.logarAviso($"Não foi possivel identificar uma pessoa cadastrada com este ID: {id}. Requirido por: {currentUser}");
                return StatusCode(404);
            }
            _logger.logarInfo($"Localizado pessoa com ID: {id}. Ação feita por: [{currentUser}]");
            return StatusCode(200, pessoa);
        }

        // POST 
        /// <summary>
        /// Cria uma nova pessoa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///       "nome": {
        ///         "primeiroNome": "Roger",
        ///         "sobreNome": "Yakubets"
        ///       },
        ///       "endereco": {
        ///         "cidade": "Lagoas",
        ///         "bairro": "Riveira",
        ///         "rua": "Manoel Antonio Siqueira",
        ///         "cep": "8852513",
        ///         "numero": 254
        ///       },
        ///       "telefone": {
        ///         "codigoPais": "55",
        ///         "codigoDeArea": "48",
        ///         "numero": "99542365"
        ///       },
        ///       "cpf": {
        ///         "cpf": "01234567891"
        ///       }
        ///     }
        ///
        /// </remarks>
        /// <returns>Codigo 200 dizendo que o usuario foi criado com sucesso!</returns>
        /// <response code="200">Retorna uma mensagem informando que foi criado com sucesso</response>
        /// <response code="500">Retorna uma mensagem informando que não foi possivel criar o usuario, erro interno</response>
        /// <response code="400">Retorna uma mensagem informando que não foi possivel criar o usuario, argumento invalido</response>
        [HttpPost("pessoa/adicionar")]
        [Authorize(Roles="ADMIN")]
        public async Task<IActionResult> adicionarPessoa(Pessoa model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid)
            {
                _logger.logarAviso($"Não foi possivel criar a pessoa com este modelo. [Modelo Invalido]. Requirido por [{currentUser}]");
                return StatusCode(400, "Argumento invalido");
            }
            var result = await _repo.criarPessoa(model);
            if (result)
            {
                _logger.logarInfo($"Adicionado pessoa com sucesso. Ação feita por: [{currentUser}]");
                return StatusCode(200, "Pessoa adicionada com sucesso!");
            }
            _logger.logarErro($"Não foi possivel adicionar o pessoa, devido a um problema interno. Requirido por: [{currentUser}]");
            return StatusCode(500, "Não foi possivel adicionar a pessoa!");
        }


        /// <summary>
        /// Atualiza o endereço de uma pessoa.
        /// </summary>
        /// <remarks>
        /// 
        /// Exemplo:
        ///     
        ///     {
        ///       "cidade": "Palhoça",
        ///       "bairro": "Bela Vista",
        ///       "rua": "Fernando Medeiros",
        ///       "cep": "885523840",
        ///       "numero": 2544
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna a pessoa com o dado atualizado</response>
        /// <response code="404">Retorna dado de pessoa não encontrada</response>
        /// <response code="400">Informa que não foi possivel atualizar devido a erro de model</response>
        [HttpPut("pessoa/atualizar/endereco/{id}")]
        [Authorize(Roles="ADMIN")]
        public async Task<IActionResult> atualizarPessoaEndereco(int id, Endereco model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            if (!ModelState.IsValid)
            {
                _logger.logarAviso($"Não foi possivel atualizar o endereço do ID: {id}. [Modelo Invalido]. Requirido por: [{currentUser}]");
                return StatusCode(400, ModelState);
            }
            var result = await _repo.atualizarEndereco(id, model);
            if (result == null)
            {
                _logger.logarAviso($"Não existe uma pessoa para este ID: {id}");
                return StatusCode(404);
            }
            _logger.logarInfo($"Realizado atualização de endereço no ID: {id}. Ação feita por: [{currentUser}]");
            return StatusCode(200, result);
        }

        /// <summary>
        /// Atualiza o telefone de uma pessoa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///     
        ///     {
        ///       "codigoPais": "55",
        ///       "codigoDeArea": "11",
        ///       "numero": "84563368"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna a pessoa com o dado atualizado</response>
        /// <response code="404">Retorna dado de pessoa não encontrada</response>
        [HttpPut("pessoa/atualizar/telefone/{id}")]
        [Authorize(Roles="ADMIN")]
        public async Task<IActionResult> atualizarTelefone(int id, Telefone model)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!ModelState.IsValid)
            {
                _logger.logarErro($"Não foi possivel atualizar telefone do ID: {id}. [Modelo invalido]. Requirido por: [{currentUser}]");
            }
            var result = await _repo.atualizarTelefone(id, model);
            if (result == null)
            {
                _logger.logarAviso($"Não foi possivel atualizar telefone para o ID: {id}. Requirido por: [{currentUser}]");
                return StatusCode(404);
            }
            _logger.logarInfo($"Realizado atualizacao de telefone do ID: {id}. Ação realizado por: [{currentUser}]");
            return StatusCode(200, result);
        }

        /// <summary>
        /// Ira realizar a removeção de uma pessoa a partir de um ID.
        /// </summary>
        [HttpDelete("pessoa/deletar/{id}")]
        [Authorize(Roles="ADMIN")]
        public async Task<IActionResult> deletarPessoa(int id)
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var result = await _repo.deletarPessoa(id);
            if (result)
            {
                _logger.logarInfo($"Deletado usuario com ID: {id}. Ação realizado por: [{currentUser}]");
                return StatusCode(200, "Pessoa removida com sucesso");
            }
            _logger.logarErro($"Erro ao tentar realizar remoção da pessoa com ID: {id}. requirido por: [{currentUser}]");
            return StatusCode(500, "Não foi possivel remover a pessoa");
        }
    }
}