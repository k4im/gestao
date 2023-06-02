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

        /// <summary>
        /// Retorna uma lista paginada de Pessoas.
        /// </summary>
        [HttpGet("pessoas/{pagina?}/{resultado?}"), AllowAnonymous]
        public async Task<IActionResult> buscarPessoas(int pagina = 1, float resultado = 5)
        {
            var pessoas = await _repo.buscarPessoas(pagina, resultado);
            if (pessoas == null) return NotFound();
            return StatusCode(200, pessoas);
        }

        /// <summary>
        /// Ira retornar uma pessoa a partir de um ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns code="200">Retorna a pessoa pertencente ao id</returns>
        /// <returns code="404">Informa que não existe uma pessoa com tal ID</returns>
        [HttpGet("pessoa/{id}"),
        AllowAnonymous]
        public async Task<IActionResult> buscarPessoa(int id)
        {
            var pessoa = await _repo.buscarPessoaId(id);
            if (pessoa == null) return NotFound();
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
        /// <response code="500">Retorna uma mensagem informando que não foi possivel criar o usuario</response>
        [HttpPost("pessoa/adicionar")]
        public async Task<IActionResult> adicionarPessoa(Pessoa model)
        {
            var result = await _repo.criarPessoa(model);
            return (result) ? StatusCode(200, "Pessoa adicionada com sucesso!") :
            StatusCode(500, "Não foi possivel adicionar a pessoa!");
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
        [HttpPut("pessoa/atualizar/endereco/{id}")]
        public async Task<IActionResult> atualizarPessoaEndereco(int id, Endereco model)
        {
            var result = await _repo.atualizarEndereco(id, model);
            if (result == null) return StatusCode(404);
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
        public async Task<IActionResult> atualizarTelefone(int id, Telefone model)
        {
            var result = await _repo.atualizarTelefone(id, model);
            if (result == null) return StatusCode(404);
            return StatusCode(200, result);
        }

        /// <summary>
        /// Ira realizar a removeção de uma pessoa a partir de um ID.
        /// </summary>
        [HttpDelete("pessoa/deletar/{id}"),
        Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> deletarPessoa(int id)
        {
            var result = await _repo.deletarPessoa(id);
            return (result) ? StatusCode(200, "Pessoa removida com sucesso") :
            StatusCode(500, "Não foi possivel remover a pessoa");
        }
    }
}