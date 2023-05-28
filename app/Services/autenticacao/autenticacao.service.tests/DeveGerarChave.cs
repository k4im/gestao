using autenticacao.service.tests.Helpers;

namespace autenticacao.service.tests
{
    public class DeveGerarChave
    {
        [Fact]
        public async void deve_gerar_chave_de_acesso()
        {
            //Arrange 
            var fakeManager = new FakeUserManager();
            var chaveM = new ChaveManager(fakeManager);

            //Act
            var result = await chaveM.gerarChaveDeAcesso();

            //Assert 
            Assert.NotNull(result);
        }
    }
}