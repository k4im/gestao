namespace projeto.service.Models
{
    public class ProdutosDisponiveis
    {
        public ProdutosDisponiveis(int id, string nome, int quantidade)
        {
            Id = id;
            Nome = nome;
            Quantidade = quantidade;
        }

        [Key]
        public int Id { get; }
        public string Nome { get; }
        public int Quantidade { get; }
    }
}