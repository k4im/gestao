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
            Assert.IsType<Produto>(result);
        }

        [Fact]
        public async Task ao_adicionar_produto_deve_retornar_true()
        {
            //Arrange
            var context = new DataContext(_contextOpt.Options);
            var novoProduto = new Produto("produto", 55.5, 4);
            var _repo = new RepoEstoque(context);
            var expect = true;

            //Act
            var result = await _repo.adicionarProduto(novoProduto);

            //Assert
            Assert.Equal(expect, result);
        }

        [Fact]
        public async Task ao_deletar_produto_deve_retornar_true()
        {
            //Arrange
            var context = new DataContext(_contextOpt.Options);
            var model = new Produto("nome", 55.5, 5);
            var _repo = new RepoEstoque(context);
            await _repo.adicionarProduto(model);
            var expect = true;

            //Act
            var result = await _repo.removerProduto(1);

            //Assert
            Assert.Equal(expect, result);

        }

        [Fact]
        public async Task ao_realizar_update_deve_retornar_true()
        {
            //Arrange
            var context = new DataContext(_contextOpt.Options);
            var model = new Produto("nome", 55.5, 5);
            var modelUpdt = new Produto("nOOme", 55.5, 5);
            var _repo = new RepoEstoque(context);
            await _repo.adicionarProduto(model);
            var expect = true;

            //Act
            var result = await _repo.atualizarProduto(1, modelUpdt);

            //Assert
            Assert.Equal(expect, result);
        }
    }
}
