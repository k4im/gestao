using estoque.service.Data;
using estoque.service.Repository;
using estoque.service.tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace estoque.service.tests
{
    public class RepoEstoqueTeste
    {
        DbContextOptionsBuilder _contextOpt = FakeDbOptions.factoryDbInMemory();

        [Fact]
        public async Task deve_retornar_produto()
        {
            // Arrange 
            var context = new DataContext(_contextOpt.Options);
            var _repo = new RepoEstoque(context);

            //Act
            var result = await _repo.buscarProdutoId(2);

            //Assert
            Assert.IsType<Object>(result);
        }
    }
}