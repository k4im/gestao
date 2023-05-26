namespace autenticacao.service.tests
{
    public class DeveCriarEnderecoTest
    {
        [Theory]
        [InlineData("lages", "Penha", "string", "1234567", 0)]
        //           Cidade,  Bairro,   Rua,      Cep,   Numero da casa
        public void DeveCriarEndereco(string cidade, string bairro, string rua, string cep, int numero)
        {
            var validarCidade = new Endereco(cidade, bairro, rua, cep, numero);
        }
    }
}