namespace Produto.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Produto.Data.Repositories;


public static class DependencyInjection
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProdutosRepository, ProdutosRepository>();
        services.AddDbContextPool<ProdutosContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("ProdutoContext"));
        });

        return services;
    }
}