namespace autenticacao.service.tests
{
    public class DeveCriarTelefone
    {
        [Theory]
        [InlineData("22", "52", "5555")]
        //        pais,    area,  numero
        public void DeveRetornarErroNumeroInvalido(string pais, string area, string numero)
        {
            var telefone = new Telefone(pais, area, numero);
        }
    }
}