namespace estoque.service.Models
{
    public class Response<T>
    {
        public Response(List<T> data, int paginaAtual, int totalDePaginas)
        {
            Data = data;
            PaginaAtual = paginaAtual;
            TotalDePaginas = totalDePaginas;
        }

        public List<T> Data { get; } = new List<T>();
        public int PaginaAtual { get; }
        public int TotalDePaginas { get; }
    }
}