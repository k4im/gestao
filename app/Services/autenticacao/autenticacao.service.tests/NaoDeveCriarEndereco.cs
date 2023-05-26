namespace autenticacao.service.tests
{
    public class NaoDeveCriarEnderecoTest
    {
        public Endereco erro { get; set; } = new Endereco("cidade", "bairro", "rua","1230040", 0);
        [Theory]
        [InlineData("")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroCidadeVaziaInvalida(string cidade)
        {
            
            var error = Assert.Throws<CampoVazio>(() => erro.VerificarCidade(cidade));
            Assert.Equal("A cidade não pode estar vazia!", error.Message);
            
        }
        [Theory]
        [InlineData("")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroBairroVazioInvalida(string bairro)
        {
            var error = Assert.Throws<CampoVazio>(() => erro.VerificarBairro(bairro));
            Assert.Equal("O bairro não pode estar vazio!", error.Message);
            
        }

        [Theory]
        [InlineData("")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroRuaVaziaInvalida(string rua)
        {
            var error = Assert.Throws<CampoVazio>(() => erro.VerificarRua(rua));
            Assert.Equal("A rua não pode estar vazia!", error.Message);
            
        }
    
        [Theory]
        [InlineData("")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroCepVazioInvalida(string cep)
        {
            var error = Assert.Throws<CampoVazio>(() => erro.VerificarCep(cep));
            Assert.Equal("O cep não pode estar vazio!", error.Message);
            
        }

        [Theory]
        [InlineData("12323@")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroCidadeInvalida(string cidade)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.VerificarCidade(cidade));
            Assert.Equal("A cidade não pode conter caracteres especiais", error.Message);
        }
        
        [Theory]
        [InlineData("12323@")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroBairroInvalido(string bairro)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.VerificarBairro(bairro));
            Assert.Equal("O bairro não pode conter caracteres especiais", error.Message);
        }

/*
        Necessário estar realizando adequação
        
        [Theory]
        [InlineData("12323@")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroRuaInvalido(string rua)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.VerificarRua(rua));
            Assert.Equal("O bairro não pode conter caracteres especiais", error.Message);
        }
        
*/
        [Theory]
        [InlineData("12323@")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroCepInvalido(string cep)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.VerificarCep(cep));
            Assert.Equal("O cep não pode conter caracteres especiais", error.Message);
        }
    }
}
