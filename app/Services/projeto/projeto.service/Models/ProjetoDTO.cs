namespace projeto.service.Models
{
    public class ProjetoDTO
    {
        public string Nome { get; set; }
        public int QuantidadeDeChapa { get; set; }
        public ChapaUtilizada Chapa { get; set; }
    }
}