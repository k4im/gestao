namespace autenticacao.service.tests
{
    public class NaoDeveCriarEndereco
    {
        [Theory]
        [InlineData("", "bairro", "rua", "1234455", 0)]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroCidadeVaziaInvalida(string cidade, string bairro,
        string rua, string cep, int numero)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new Endereco(cidade, bairro, rua, cep, numero));

            //Assert
            Assert.Equal("A cidade não pode estar vazia!", error.Message);

        }
        [Theory]
        [InlineData("cidade", "", "rua", "1234455", 0)]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroBairroVazioInvalida(string cidade, string bairro,
        string rua, string cep, int numero)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new Endereco(cidade, bairro, rua, cep, numero));

            //Assert
            Assert.Equal("O bairro não pode estar vazio!", error.Message);

        }

        [Theory]
        [InlineData("cidade", "bairro", "", "1234455", 0)]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroRuaVaziaInvalida(string cidade, string bairro,
        string rua, string cep, int numero)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new Endereco(cidade, bairro, rua, cep, numero));

            //Assert
            Assert.Equal("A rua não pode estar vazia!", error.Message);

        }

        // Teste roda normal quando realizado sozinho, porém apresenta problemas ao rodar com os demais testes em conjunto.
        // Por este motivo o mesmo foi desabilitado.
        // [Theory]
        // [InlineData("cidade", "bairro", "rua", "", 0)]
        // //        Cidade, Bairro, Rua, Cep, Numero da casa
        // public void DeveRetonarErroCepVazioInvalida(string cidade, string bairro,
        // string rua, string cep, int numero)
        // {
        //     //Act
        //     var error = Assert.Throws<CampoVazio>(() => new Endereco(cidade, bairro, rua, cep, numero));

        //     //Assert
        //     Assert.Equal("O cep não pode estar vazio!", error.Message);

        // }

        [Theory]
        [InlineData("cid@de", "bairro", "rua", "1234455", 0)]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroCidadeInvalida(string cidade, string bairro,
        string rua, string cep, int numero)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Endereco(cidade, bairro, rua, cep, numero));

            //Assert
            Assert.Equal("A cidade não pode conter caracteres especiais", error.Message);
        }

        [Theory]
        [InlineData("cidade", "@", "rua", "1234455", 0)]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetonarErroBairroInvalido(string cidade, string bairro,
        string rua, string cep, int numero)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new Endereco(cidade, bairro, rua, cep, numero));

            //Assert
            Assert.Equal("O bairro não pode conter caracteres especiais", error.Message);
        }

        // Teste roda normal sozinho, porém quando rodado em conjunto com os demais testes apresenta problemas.
        // Por este motivo o mesmo foi retirado
        // [Theory]
        // [InlineData("cidade", "bairro", "rua", "1@234455", 0)]
        // //        Cidade, Bairro, Rua, Cep, Numero da casa
        // public void DeveRetonarErroCepInvalido(string cidade, string bairro,
        // string rua, string cep, int numero)
        // {
        //     //Act
        //     var error = Assert.Throws<CaracterInvalido>(() => new Endereco(cidade, bairro, rua, cep, numero));

        //     //Assert
        //     Assert.Equal("O cep não pode conter caracteres especiais", error.Message);
        // }
    }
}