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

        [HttpGet("projetos/{pagina?}/{resultadoPorPagina?}"), ValidateAntiForgeryToken, Authorize(Roles = "ADMIN, ATENDENTE")]
        public async Task<IActionResult> GetAllProjects(int pagina = 1, float resultadoPorPagina = 5)
        {
            var projetos = await _repo.BuscarProdutos(pagina, resultadoPorPagina);
            return Ok(projetos);
        }

        [HttpGet("projeto/{id?}"), ValidateAntiForgeryToken, Authorize(Roles = "ADMIN, ATENDENTE")]
        public async Task<IActionResult> GetById(int? id)
        {
            var item = await _repo.BuscarPorId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("Create"), ValidateAntiForgeryToken, Authorize(Roles = "ADMIN, ATENDENTE")]
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


        [HttpPut("update/{id}"), ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
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

        [HttpDelete("delete/{id}"), ValidateAntiForgeryToken, Authorize(Roles = "ADMIN")]
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