using estoque.service.Exceptions;

namespace estoque.service.tests
{
    public class ProdutoTest
    {
        [Theory]
        [InlineData("O nome não pode estar vazio!")]
        public void retonar_erro_ao_cirar_produto_nome(string message)
        {
            //Act 
            var result = Assert.Throws<CampoVazio>(() => new Produto("", 55.55, 5));

            //Assert
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("O nome não pode conter caracteres especiais")]
        public void retonar_erro_ao_criar_produto_nome(string message)
        {
            //Act
            var result = Assert.Throws<CaracterInvalido>(() => new Produto("@", 55.55, 5));

            //Assert
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("O valor não pode ser um valor menor que 0")]
        public void retornar_erro_ao_definir_valor_produto(string message)
        {
            //Act
            var result = Assert.Throws<Exception>(() => new Produto("nome", -1, 5));

            //Assert 
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("A quantidade não pode ser um valor menor que 0")]
        public void retonar_erro_ao_definir_quantidade_produto(string message)
        {
            //Act
            var result = Assert.Throws<Exception>(() => new Produto("nome", 5, -1));

            //Assert
            Assert.Equal(message, result.Message);
        }
    }
}