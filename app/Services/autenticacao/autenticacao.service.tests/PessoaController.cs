using Microsoft.AspNetCore.Mvc;
namespace autenticacao.service.tests
{
    public class PessoaControllerTest : ControllerBase
    {
        [Fact]
        public async Task get_deve_retonar_status_200()
        {
            //Arrange
            var mockRepo =  new Mock<IRepoPessoa>();
            var pessoaController = new PessoaControllers(mockRepo.Object);
            var except = StatusCode(200);
            //Act
            var result = await pessoaController.buscarPessoas(1, 5);
            var actual = result as object;
            
            //Assert
            Assert.Equal(except, actual);
        }
    }
}