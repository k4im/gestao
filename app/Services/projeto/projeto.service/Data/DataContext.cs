namespace projeto.service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API for specifying concurrency token
            modelBuilder.Entity<Projeto>()
                .Property(projeto => projeto.RowVersion)
                .IsConcurrencyToken();


            modelBuilder.Entity<Projeto>(builder =>
            {
                builder.OwnsOne<StatusProjeto>(projeto => projeto.Status)
                .Property(status => status.Status)
                .HasColumnName("Status");
            });
        }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<ProdutosDisponiveis> ProdutosEmEstoque { get; set; }
    }
}