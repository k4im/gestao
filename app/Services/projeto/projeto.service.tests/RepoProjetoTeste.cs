using projeto.service.Data;
using projeto.service.Models;
using projeto.service.Repository;
using projeto.service.tests.Helpers;

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
    }
}