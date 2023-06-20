namespace projeto.service.tests
{
    public class RepoProjetoTeste
    {
        [Fact]
        public async void deve_retornar_true_adicionar_projeto()
        {
            //Arrange
            var context = new DataContext(FakeDbOptions.factoryDbInMemory().Options);
            var _repo = new RepoProjetos(context);
            var model = FakeProjeto.factoryProjeto();

            //Act
            var result = await _repo.CriarProjeto(model);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void deve_retornar_true_ao_atualizar_status()
        {
            //Arrange
            var context = new DataContext(FakeDbOptions.factoryDbInMemory().Options);
            var _repo = new RepoProjetos(context);
            var model = FakeProjeto.factoryProjeto();
            await _repo.CriarProjeto(model);
            var modelStatus = FakeStatus.factoryStatus();

            //Act
            var result = await _repo.AtualizarStatus(modelStatus, 1);

            //
            Assert.True(result);
        }

        [Fact]
        public async void deve_retornar_true_ao_deletar_projeto()
        {
            //Arrange
            var context = new DataContext(FakeDbOptions.factoryDbInMemory().Options);
            var _repo = new RepoProjetos(context);
            var model = FakeProjeto.factoryProjeto();
            await _repo.CriarProjeto(model);

            //Act
            var result = await _repo.DeletarProjeto(1);

            //
            Assert.True(result);
        }

        [Fact]
        public async void deve_retornar_projeto_por_id()
        {
            //Arrange
            var context = new DataContext(FakeDbOptions.factoryDbInMemory().Options);
            var _repo = new RepoProjetos(context);
            var model = FakeProjeto.factoryProjeto();
            await _repo.CriarProjeto(model);

            //Act
            var result = await _repo.BuscarPorId(1);

            //
            Assert.IsType<Projeto>(result);
        }

        [Fact]
        public async void deve_retornar_lista_projetos()
        {
            //Arrange
            var context = new DataContext(FakeDbOptions.factoryDbInMemory().Options);
            var projetos = new List<Projeto> {
                FakeProjeto.factoryProjeto(),
                FakeProjeto.factoryProjeto(),
                FakeProjeto.factoryProjeto(),
            };

            var repo = new RepoProjetos(context);
            foreach (var projeto in projetos) await repo.CriarProjeto(projeto);
            var response = new Response<Projeto>(projetos, 1, 3);

            var expect = response.Data.Count;

            //Act
            var result = await repo.BuscarProdutos(1, 3);
            var actual = result.Data.Count;

            //Assert
            Assert.Equal(expect, actual);
        }
    }
}