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
            var controller = new ProdutosController(mockRepo.Object);
            var novoProduto = new Produto("Chapa Branca", 55.55, 1);

            //Act
            var result = (ObjectResult)await controller.adicionarProduto(novoProduto);

            //Assert
            result.StatusCode.Should().Be(201);
        }
    }
}