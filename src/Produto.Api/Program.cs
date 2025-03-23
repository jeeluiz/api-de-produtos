using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Produto.Api.Middlewares;
using Produto.Api;
using Produto.Application;
using Produto.Application.Dtos;
using Produto.Application.Services;
using Produto.Data;

/* ------------------------ Configuração de Services ------------------------ */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDb(builder.Configuration);
builder.Services.AddServices();

var app = builder.Build();

/* ----------------------- Configuração de Middlewares ---------------------- */

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/openapi/v1.json", "Produto API V1");
    });
}

// Adiciona o middleware de tratamento global de exceções
app.UseMiddleware<GlobalExceptionMiddleware>();
/* -------------------------------- Endpoints ------------------------------- */

// Redirects para o swagger (facilitar testes)
app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
    .ExcludeFromDescription();
app.MapGet("/index.html", () => Results.Redirect("/swagger/index.html"))
    .ExcludeFromDescription();

app.MapGet("/exception", () => {
    throw new Exception("Teste de exceção");
});

app.MapGet(
    "/produtos",
    async ([FromServices] ProdutosService service,
           [FromQuery] int? page = 1,
           [FromQuery] int? pageSize = 10,
           [FromQuery] string? search = null,
           [FromQuery] string? sortBy = null) => {
               if (page < 1) page = 1;
               if (pageSize < 1) pageSize = 1;
               if (pageSize > 100) pageSize = 100;
               if (string.IsNullOrEmpty(search)) search = null;
               else if (search.Length > 50) search = search[..50];
               var sortDict = sortBy.ParseSortBy();
               var result = await service.GetAsync(page, pageSize, search, sortDict);
               return Results.Ok(result);
           })
 .WithOpenApi(config => {
     config.Parameters = [
         new OpenApiParameter {
                Name = "page",
                Description = "Número da página. Padrão: 1",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "integer" },
            },
            new OpenApiParameter {
                Name = "pageSize",
                Description = "Número de itens por página. Padrão: 10",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "integer" },
            },
            new OpenApiParameter {
                Name = "search",
                Description = "Texto para busca. Padrão: null",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "string" },
            },
            new OpenApiParameter {
                Name = "sortBy",
                Description = "Campo para ordenação. Formato: {campo1}:{asc|desc}, {campo2}:{asc|desc}. Padrão: null",
                Required = false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "string" },
            }
     ];
     config.Summary = "Retorna todos os produtos";
     config.Description = "Retorna todos os produtos cadastrados no sistema";
     config.Tags = [new OpenApiTag { Name = "Produtos" }];
     config.OperationId = "GetProdutos";

     return config;
 });
    //.WithName("GetProdutos")
    //.Produces<AppPagedResult<ProdutoDto>>(StatusCodes.Status200OK);

app.MapGet(
    "/produtos/{idOuNome}",
    async ([FromRoute] string idOuNome,
           [FromServices] ProdutosService service) => {
               var result = await service.GetByIdAsync(idOuNome);
               return result.StatusCode switch {
                   AppResultStatus.Sucesso => Results.Ok(result),
                   AppResultStatus.NaoEncontrado => Results.NotFound(result),
                   _ => Results.BadRequest(result)
               };
           })
    .WithOpenApi()
    .WithName("GetProdutoById")
    .WithSummary("Retorna um produto pelo ID")
    .WithDescription("Retorna um produto cadastrado no sistema pelo ID")
    .WithTags("Produtos")
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status200OK)
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status404NotFound);

app.MapPost(
    "/produtos",
    async ([FromBody] ProdutoDto produtoDto,
           [FromServices] ProdutosService service) => {
               var result = await service.CreateAsync(produtoDto);
               return result.StatusCode switch {
                   AppResultStatus.Sucesso => Results.Created($"/produtos/{result.Data?.Id}", result),
                   AppResultStatus.Erro => Results.BadRequest(result),
                   _ => Results.BadRequest(result)
               };
           })
    .WithOpenApi()
    .WithName("CreateProduto")
    .WithSummary("Cria um novo produto")
    .WithDescription("Cria um novo produto no sistema")
    .WithTags("Produtos")
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status201Created)
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status400BadRequest);

app.MapPut(
    "/produtos/{idOuNome}",
    async ([FromRoute] string idOuNome,
           [FromBody] ProdutoDto produtoDto,
           [FromServices] ProdutosService service) => {
               if (idOuNome == string.Empty) {
                   return Results.BadRequest(new AppResult<ProdutoDto>(
                        message: "ID do produto não pode ser vazio",
                        statusCode: AppResultStatus.Erro,
                        success: false));
               }
               var result = await service.UpdateAsync(idOuNome, produtoDto);
               return result.StatusCode switch {
                   AppResultStatus.Sucesso => Results.Ok(result),
                   AppResultStatus.NaoEncontrado => Results.NotFound(result),
                   AppResultStatus.Erro => Results.BadRequest(result),
                   _ => Results.BadRequest(result)
               };
           })
    .WithOpenApi()
    .WithName("UpdateProduto")
    .WithSummary("Atualiza um produto")
    .WithDescription("Atualiza um produto cadastrado no sistema")
    .WithTags("Produtos")
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status200OK)
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status404NotFound)
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status400BadRequest);

app.MapDelete(
    "/produtos/{id}",
    async ([FromRoute] Guid id,
           [FromServices] ProdutosService service) => {
               var result = await service.DeleteAsync(id);
               return result.StatusCode switch {
                   AppResultStatus.Sucesso => Results.NoContent(),
                   AppResultStatus.NaoEncontrado => Results.NotFound(result),
                   _ => Results.BadRequest(result)
               };
           })
    .WithOpenApi()
    .WithName("DeleteProduto")
    .WithSummary("Deleta um produto")
    .WithDescription("Deleta um produto cadastrado no sistema")
    .WithTags("Produtos")
    .Produces(StatusCodes.Status204NoContent)
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status404NotFound)
    .Produces<AppResult<ProdutoDto>>(StatusCodes.Status400BadRequest);
/* -------------------------------------------------------------------------- */

app.Run();
