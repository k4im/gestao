using autenticacao.service.Models.Tokens;
using autenticacao.service.RefreshManagers;
using autenticacao.service.tests.Helpers;
using AutoMapper;
using Moq;
using autenticacao.service.AutoMapper;

namespace autenticacao.service.tests
{
    public class RefreshManagerTest
    {
        DbContextOptionsBuilder _contextOptions = FakeDbOptions.factoryDbInMemory();
        DataContext _context;
        
        [Theory]
        [InlineData("teste")]
        public void deve_gerar_refresh_token(string chave)
        {
            //Arrange
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileMapper());
            });
            var mapper = mockMapper.CreateMapper();
            _context = new DataContext(_contextOptions.Options);
            var repoMM = new RefreshManager(_context, mapper);

            //Act
            var result = repoMM.GerarRefreshToken(chave);
        
            //Assert
            Assert.IsType<RefreshToken>(result);
        }

        [Theory]
        [InlineData("teste")]
        public async Task deve_adicionar_refresh_token(string chave)
        {
            //Arrange
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileMapper());
            });
            var mapper = mockMapper.CreateMapper();
            _context = new DataContext(_contextOptions.Options);
            var repoMM = new RefreshManager(_context, mapper);

            //Act 
            var tokenTeste = repoMM.GerarRefreshToken(chave);
            var result = await repoMM.SalvarRefreshToken(tokenTeste);

            //Assert
            Assert.True(result);

        }

        [Theory]
        [InlineData("teste")]
        public async Task deve_retornar_refresh_token_table(string chave)
        {
            //Arrange
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileMapper());
            });
            var mapper = mockMapper.CreateMapper();
            _context = new DataContext(_contextOptions.Options);
            var repoMM = new RefreshManager(_context, mapper);
        
            //Act 
            var tokenTeste = repoMM.GerarRefreshToken(chave);
            var addToken = await repoMM.SalvarRefreshToken(tokenTeste);
            var result = await repoMM.BuscarRefreshToken(chave, tokenTeste.Token);

            //Assert
            Assert.IsType<RefreshToken>(result);
        }


        [Theory]
        [InlineData("teste")]
        public async Task deve_deletar_refresh_token(string chave)
        {
            //Arrange
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileMapper());
            });
            var mapper = mockMapper.CreateMapper();
            _context = new DataContext(_contextOptions.Options);
            var repoMM = new RefreshManager(_context, mapper);
        
            //Act 
            var tokenTeste = repoMM.GerarRefreshToken(chave);
            var addToken = await repoMM.SalvarRefreshToken(tokenTeste);
            var result = await repoMM.DeletarRefreshToken(tokenTeste.Token);

            //Assert
            Assert.True(result);
        }
    }
}