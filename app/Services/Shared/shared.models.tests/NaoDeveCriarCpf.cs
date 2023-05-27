using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace shared.models.tests
{
    public class NaoDeveCriarCpf
    {
        [Theory]
        [InlineData("")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetornarErroCpfVazio(string cpf)
        {
            //Act
            var error = Assert.Throws<CampoVazio>(() => new CadastroPessoaFisica(cpf));
            
            //Assert
            Assert.Equal("O cpf não pode estar nulo!", error.Message);
            
        }

        [Theory]
        [InlineData("@")]
        [InlineData("asd")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetornarErroCpfInvalido(string cpf)
        {
            //Act
            var error = Assert.Throws<CaracterInvalido>(() => new CadastroPessoaFisica(cpf));
            
            //Assert
            Assert.Equal("Insira apenas os numeros do cpf", error.Message);
            
        }


        [Theory]
        [InlineData("0120362581")]
        //        Cidade, Bairro, Rua, Cep, Numero da casa
        public void DeveRetornarCpfPrecisaConterOnzeNumeros(string cpf)
        {
            //Act
            var error = Assert.Throws<Exception>(() => new CadastroPessoaFisica(cpf));
            
            //Assert
            Assert.Equal("O cpf deve conter no minimo 11 caracteres!", error.Message);
            
        }
    }
}