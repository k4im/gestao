namespace shared.models.tests
{
    public class NaoDeveCriarNome
    {
       
        [Theory]
        [InlineData("", "segundo nome")]
        public void DeveRetornarNomeVazioInvalido(string primeiroNome, string segundo)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new Nome(primeiroNome, segundo));
            
            //Assert
            Assert.Equal("O nome não pode estar vazio!", error.Message);
        }

        [Theory]
        [InlineData("primeironome", "")]
        public void DeveRetonarSobreNomeVazioInvalido(string primeiroNome, string segundo)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new Nome(primeiroNome, segundo));
            
            //Assert
            Assert.Equal("O sobrenome não pode estar vazio!", error.Message);
        }


        [Theory]
        [InlineData("@", "teste")]
        public void DeveRetornarNomeInvalido(string primeiroNome, string segundo)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Nome(primeiroNome, segundo));
            
            //Assert
            Assert.Equal("O nome não pode conter caracteres especiais", error.Message);
        }

        [Theory]
        [InlineData("teste", "@")]
        public void DeveRetonarSobreNomeInvalido(string primeiroNome, string segundo)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Nome(primeiroNome, segundo));
            
            //Assert
            Assert.Equal("O sobrenome não pode conter caracteres especiais", error.Message);
        }
    }
}