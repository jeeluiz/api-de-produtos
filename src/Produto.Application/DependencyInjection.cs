namespace Produto.Application;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using Produto.Application.Dtos;
using Produto.Application.Services;
using Produto.Application.Validators;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ProdutosService>();
        services.AddScoped<IValidator<ProdutoDto>, ProdutoValidator>();

        return services;
    }
}