namespace shared.models.tests
{
    public class NaoDeveCriarTelefone
    {
        [Theory]
        [InlineData("@", "Teste", ",.,")]
        //        pais,    area,  numero
        public void DeveRetonarErroCaractereEspecial(string pais, string area, string numero)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Telefone(pais, area, numero));
            
            
            Assert.Equal("Codigo do país precisa conter apenas numeros", error.Message);
        }
       
        [Theory]
        [InlineData("22", "Teste", ",.,")]
        //        pais,    area,  numero
        public void DeveRetornarErroAreaInvalida(string pais, string area, string numero)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Telefone(pais, area, numero));
            
            //Assert
            Assert.Equal("Codigo de area precisa conter apenas numeros", error.Message);
        }


        [Theory]
        [InlineData("22", "52", ".ç.")]
        //        pais,    area,  numero
        public void DeveRetornarErroNumeroInvalido(string pais, string area, string numero)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Telefone(pais, area, numero));
            
            //Assert
            Assert.Equal("Numero de telefone precisa conter apenas numeros", error.Message);
        }

        [Theory]
        [InlineData("22", "", "5959595")]
        public void DeveRetornarAreaInvalida(string pais, string area, string numero)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new Telefone(pais, area, numero));
            
            //Assert
            Assert.Equal("O DDD precisa ser preenchido", error.Message);
        }
    }
}
