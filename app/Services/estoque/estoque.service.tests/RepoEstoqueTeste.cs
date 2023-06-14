using estoque.service.Data;
using estoque.service.Models;
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
            var novoProduto = new Produto("Produto", 55.5, 4);
            var _repo = new RepoEstoque(context);
            await _repo.adicionarProduto(novoProduto);

            //Act
            var result = await _repo.buscarProdutoId(1);

            //Assert
            Assert.IsType<Object>(result);
        }
    }
}