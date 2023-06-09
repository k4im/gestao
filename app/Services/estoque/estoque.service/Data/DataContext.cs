namespace estoque.service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Projeto> Projetos { get; set; }
    }
}