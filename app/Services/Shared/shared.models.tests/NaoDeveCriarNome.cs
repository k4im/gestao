using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shared.models.Exceptions;
using shared.models.ValueObjects;
using Xunit;

namespace shared.models.tests
{
    public class NaoDeveCriarNome
    {

        Nome erro { get; set; } = new Nome("teste", "teste");
        
        [Theory]
        [InlineData("")]
        public void DeveRetornarNomeVazioInvalido(string primeiroNome)
        {
            var error = Assert.Throws<CampoVazio>(() => erro.validarPrimeiroNome(primeiroNome));
            Assert.Equal("O nome não pode estar vazio!", error.Message);
        }

        [Theory]
        [InlineData("")]
        public void DeveRetonarSobreNomeVazioInvalido(string sobreNome)
        {
            var error = Assert.Throws<CampoVazio>(() => erro.validarSobreNome(sobreNome));
            Assert.Equal("O sobrenome não pode estar vazio!", error.Message);
        }


        [Theory]
        [InlineData("@")]
        public void DeveRetornarNomeInvalido(string primeiroNome)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.validarPrimeiroNome(primeiroNome));
            Assert.Equal("O nome não pode conter caracteres especiais", error.Message);
        }

        [Theory]
        [InlineData("@")]
        public void DeveRetonarSobreNomeInvalido(string sobreNome)
        {
            var error = Assert.Throws<CaracterInvalido>(() => erro.validarSobreNome(sobreNome));
            Assert.Equal("O sobrenome não pode conter caracteres especiais", error.Message);
        }
    }
}