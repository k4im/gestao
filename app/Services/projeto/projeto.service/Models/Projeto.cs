namespace projeto.service.Models
{
    public class Projeto
    {
        protected Projeto()
        { }

        public Projeto(string nome, StatusProjeto status, DateTime dataInicio, DateTime dataEntrega, int produtoUtilizado, int quantidadeUtilizado, string descricao, double valor)
        {
            Nome = verificarNome(nome);
            Status = status;
            DataInicio = dataInicio;
            DataEntrega = dataEntrega;
            ProdutoUtilizado = produtoUtilizado;
            QuantidadeUtilizado = verificarQuantidade(quantidadeUtilizado);
            Descricao = descricao;
            Valor = valor;
        }

        [Key]
        public int Id { get; private set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType("NVARCHAR(25)")]
        public string Nome { get; private set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType("NVARCHAR(25)")]
        public StatusProjeto Status { get; private set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType("DATE")]
        public DateTime DataInicio { get; private set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType("DATE")]
        public DateTime DataEntrega { get; private set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType("NVARCHAR(150)")]
        public int ProdutoUtilizado { get; private set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public int QuantidadeUtilizado { get; private set; }

        [DataType("NVARCHAR(150)")]
        public string Descricao { get; private set; }


        [Required(ErrorMessage = "Campo obrigatório")]
        public double Valor { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; private set; }

        int verificarQuantidade(int quantidade)
        {
            if (quantidade < 0) throw new Exception("O valor da quantidade não pode ser negativo!");
            return quantidade;
        }

        double verificarValor(double valor)
        {
            if (valor < 0) throw new Exception("O valor não pode ser negativo!");
            return valor;
        }

        string verificarDescricao(string desc)
        {
            if (string.IsNullOrEmpty(desc)) throw new Exception("Campo não pode ser nulo");
            if (!Regex.IsMatch(desc, @"^[a-zA-Z ]+$")) throw new Exception("A rua não pode conter caracteres especiais");
            return desc;
        }
        string verificarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome)) throw new Exception("Campo não pode ser nulo");
            if (!Regex.IsMatch(nome, @"^[a-zA-Z ]+$")) throw new Exception("A rua não pode conter caracteres especiais");
            return nome;
        }

        public void AtualizarStatus(StatusProjeto model)
        {
            if (this.Status.Status != model.Status) this.Status.AtualizacaoDoStatus(model.Status);
        }
    }
}