namespace estoque.service.Models.ValueObject
{
    public class Produto
    {
        public Produto(NickName nomeProduto, int quantidade)
        {
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
        }

        public NickName NomeProduto { get; }

        public int Quantidade { get; }
    }
}