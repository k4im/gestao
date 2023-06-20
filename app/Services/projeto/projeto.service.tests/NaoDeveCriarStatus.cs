using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projeto.service.Models.ValueObjects;

namespace projeto.service.tests
{
    public class NaoDeveCriarStatus
    {
        [Theory]
        [InlineData("O status não pode estar nulo!")]
        public void nao_deve_criar_status_nulo(string message)
        {
            //Act
            var result = Assert.Throws<Exception>(() => new StatusProjeto(""));

            //Assert
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData("o status não pode conter caracteres especiais")]
        public void nao_deve_criar_status_invalido(string message)
        {
            //Act
            var result = Assert.Throws<Exception>(() => new StatusProjeto("@"));

            //Assert
            Assert.Equal(message, result.Message);
        }
    }
}