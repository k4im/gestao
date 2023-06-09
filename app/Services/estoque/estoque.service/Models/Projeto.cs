namespace estoque.service.Models
{
    public class Projeto
    {
        public Projeto(NickName projetoNome, string produtoUtilizado, int quantidaUtilizada, double valor, DateTime dataDeEntrega)
        {
            ProjetoNome = projetoNome;
            ProdutoUtilizado = produtoUtilizado;
            QuantidaUtilizada = quantidaUtilizada;
            Valor = valor;
            DataDeInicio = DateTime.UtcNow;
            DataDeEntrega = dataDeEntrega;
        }

        public NickName ProjetoNome { get; }
        public string ProdutoUtilizado { get; }
        public int QuantidaUtilizada { get; }
        public double Valor { get; }
        public DateTime DataDeInicio { get; }

        public DateTime DataDeEntrega { get; }
    }
}