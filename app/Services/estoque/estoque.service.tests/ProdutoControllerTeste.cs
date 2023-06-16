using estoque.service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace estoque.service.tests
{
    public class ProdutoControllerTeste
    {
        readonly Mock<IRepoEstoque> mockRepo = new Mock<IRepoEstoque>();

        [Fact]
        public async void get_produto_id_deve_retornar_status_400()
        {
            //Arrange
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var resultObj = (ObjectResult)await controller.buscarProdutoId(null);

            //Assert
            resultObj.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void get_produto_id_deve_retornar_status_404()
        {
            //Arrange 
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.buscarProdutoId(1);

            //Assert
            result.StatusCode.Should().Be(404);
        }


        [Fact]
        public async void post_adicionar_produto_deve_retornar_status_201()
        {
            //Arrange
            var novoProduto = new Produto("Chapa Branca", 55.55, 55);
            mockRepo.Setup(repo => repo.adicionarProduto(novoProduto)).ReturnsAsync(true);
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.adicionarProduto(novoProduto);

            //Assert
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async void post_adicionar_produto_deve_retornar_status_500()
        {
            //Arrange
            var novoProduto = new Produto("Chapa Branca", 55.55, 55);
            mockRepo.Setup(repo => repo.adicionarProduto(novoProduto)).ReturnsAsync(false);
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.adicionarProduto(novoProduto);

            //Assert
            result.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void put_atualizar_produto_deve_retornar_status_201()
        {
            //Arrange
            var novoProduto = new Produto("Chapa Branca", 55.55, 55);
            var produtoUpdt = new Produto("Chapa Branca", 55.55, 50);
            mockRepo.Setup(repo => repo.adicionarProduto(novoProduto)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.atualizarProduto(1, produtoUpdt)).ReturnsAsync(true);
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.atualizarProduto(1, produtoUpdt);

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void put_atualizar_produto_deve_retornar_status_500()
        {
            //Arrange
            var novoProduto = new Produto("Chapa Branca", 55.55, 55);
            var produtoUpdt = new Produto("Chapa Branca", 55.55, 50);
            mockRepo.Setup(repo => repo.adicionarProduto(novoProduto)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.atualizarProduto(1, produtoUpdt)).ReturnsAsync(false);
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.atualizarProduto(1, produtoUpdt);

            //Assert
            result.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void del_deletar_produto_deve_retornar_status_200()
        {
            var novoProduto = new Produto("Chapa Branca", 55.55, 55);
            mockRepo.Setup(repo => repo.adicionarProduto(novoProduto)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.removerProduto(1)).ReturnsAsync(true);
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.deletarProduto(1);

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void del_deletar_produto_deve_retornar_status_500()
        {
            var novoProduto = new Produto("Chapa Branca", 55.55, 55);
            mockRepo.Setup(repo => repo.adicionarProduto(novoProduto)).ReturnsAsync(true);
            mockRepo.Setup(repo => repo.removerProduto(1)).ReturnsAsync(false);
            var controller = new ProdutosController(mockRepo.Object);

            //Act
            var result = (ObjectResult)await controller.deletarProduto(1);

            //Assert
            result.StatusCode.Should().Be(500);
        }
    }
}