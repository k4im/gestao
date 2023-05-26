namespace autenticacao.service.tests
{
    public class NaoDeveCriarTelefoneTest
    {
        public Telefone erro { get; set; }  = new Telefone("22", "22", "0000");
        [Theory]
        [InlineData("@", "Teste", ",.,")]
        //        pais,    area,  numero
        public void DeveRetonarErroCaractereEspecial(string pais, string area, string numero)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.validarRegex(pais, area, numero));
            Assert.Equal("Codigo do país precisa conter apenas numeros", error.Message);
        }
       
        [Theory]
        [InlineData("22", "Teste", ",.,")]
        //        pais,    area,  numero
        public void DeveRetornarErroAreaInvalida(string pais, string area, string numero)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.validarRegex(pais, area, numero));
            Assert.Equal("Codigo de area precisa conter apenas numeros", error.Message);
        }


        [Theory]
        [InlineData("22", "52", ".ç.")]
        //        pais,    area,  numero
        public void DeveRetornarErroNumeroInvalido(string pais, string area, string numero)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.validarRegex(pais, area, numero));
            Assert.Equal("Numero de telefone precisa conter apenas numeros", error.Message);
        }

        [Theory]
        [InlineData("")]
        public void DeveRetornarAreaInvalida(string area)
        {
            var error = Assert.Throws<CampoVazio>(() => erro.validarCodigoDeArea(area));
            Assert.Equal("O DDD precisa ser preenchido", error.Message);
        }
    }
}