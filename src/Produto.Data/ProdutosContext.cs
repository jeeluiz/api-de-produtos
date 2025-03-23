namespace Produto.Data;

using Microsoft.EntityFrameworkCore;

using Produto.Data.Entities;

internal class ProdutosContext : DbContext
{
    public ProdutosContext(DbContextOptions<ProdutosContext> options) : base(options)
    {
    }

    public DbSet<ProdutoEntity> Produtos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProdutosContext).Assembly);
    }
}