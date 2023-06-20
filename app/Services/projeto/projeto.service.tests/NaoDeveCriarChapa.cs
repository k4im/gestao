using projeto.service.Models.ValueObjects;

namespace projeto.service.tests
{
    public class NaoDeveCriarChapa
    {

        [Theory]
        [InlineData("A chapa não pode estar nula!")]
        public void nao_deve_criar_chapa_utilizada_nulo(string message)
        {
            //Act
            var result = Assert.Throws<Exception>(() => new ChapaUtilizada(""));

            //Assert
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("A rua não pode conter caracteres especiais")]
        public void nao_deve_criar_chapa_utilizada_invalido(string message)
        {
            //Act
            var result = Assert.Throws<Exception>(() => new ChapaUtilizada("@"));

            //Assert
            Assert.Equal(message, result.Message);
        }

    }
}