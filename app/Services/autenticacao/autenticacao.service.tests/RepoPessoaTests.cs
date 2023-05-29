using autenticacao.service.tests.Helpers;

namespace autenticacao.service.tests
{
    public class RepoPessoaTests
    {

        DbContextOptionsBuilder _contextOptions = FakeDbOptions.factoryDbInMemory();
        DataContext _context;
        Nome _nome = FakeNome.factoryNome();
        Endereco _endereco = FakeEndereco.factoryFakeEndereco();
        Telefone _telefone = FakeTelefone.factoryTelefone();
        CadastroPessoaFisica _cpf = FakeCpf.factoryCpf();
        Fixture _fixture = new Fixture();

        [Fact]
        public async void DeveRetornarTrueAoCriarUsuario()
        {
            //Arrange
            _contextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(new Guid().ToString());
            _context = new DataContext(_contextOptions.Options);
            var repo = new RepoPessoa(_context);
            //Act
            var result = await repo.criarPessoa(new Pessoa(_nome, _endereco, _telefone, _cpf));

            //Assert
            Assert.True(result);

        }

        [Fact]
        public async void DeveRetornarListaDePessoas()
        {
            //Arrange
            _context = new DataContext(_contextOptions.Options);
            var pessoas = new List<Pessoa> {
                new Pessoa(FakeNome.factoryNome(), FakeEndereco.factoryFakeEndereco(), 
                FakeTelefone.factoryTelefone(), FakeCpf.factoryCpf()),
                new Pessoa(FakeNome.factoryNome(), FakeEndereco.factoryFakeEndereco(), 
                FakeTelefone.factoryTelefone(), FakeCpf.factoryCpf()),
                new Pessoa(FakeNome.factoryNome(), FakeEndereco.factoryFakeEndereco(), 
                FakeTelefone.factoryTelefone(), FakeCpf.factoryCpf()),
            };
            var repo = new RepoPessoa(_context);
            foreach(var pessoa in pessoas) await repo.criarPessoa(pessoa);
            var response = new Response<Pessoa>(pessoas, 1, 3);

            var expect = response.Data.Count;
            //Act
            var result = await repo.buscarPessoas(1, 3);
            var actual = result.Data.Count;

            //Assert
            Assert.Equal(expect, actual);

        }

        [Fact]
        public async void DeveDeletarPessoa()
        {
            //Arrange
            _context = new DataContext(_contextOptions.Options);
            var repo = new RepoPessoa(_context);
            await repo.criarPessoa(new Pessoa(_nome, _endereco, _telefone, _cpf));

            //Act
            var result = await repo.deletarPessoa(1);

            //Assert
            Assert.True(result);
        }


        [Fact]
        public async void DeveAcharPorId()
        {
            //Arrange
            _context = new DataContext(_contextOptions.Options);
            var repo = new RepoPessoa(_context);
            var pessoa = new Pessoa(_nome, _endereco, _telefone, _cpf);
            await repo.criarPessoa(pessoa);

            //Act
            var result = await repo.buscarPessoaId(1);

            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async void DeveAtualizarPessoa()
        {
            //Arrange
            _context = new DataContext(_contextOptions.Options);
            var repo = new RepoPessoa(_context);
            var pessoa = new Pessoa(_nome, _endereco, _telefone, _cpf);
            var atualizada = new Pessoa(_nome, _endereco, _telefone, _cpf);
            await repo.criarPessoa(pessoa);

            //Act
            var result = await repo.atualiarPessoa(1, atualizada);

            //Assert
            Assert.True(result);
        }
    }
}