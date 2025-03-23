using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Produto.Api.Middlewares;
using Produto.Api;
using Produto.Application;
using Produto.Application.Dtos;
using Produto.Application.Services;
using Produto.Data;
using System.Reflection;

/* ------------------------ Configuração de Services ------------------------ */

var builder = WebApplication.CreateBuilder(args);

/* ------------------------- Configuração de Serviços ------------------------ */

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Produto API", Version = "v1" });
    
    // Configurar documentação XML dos controllers
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Adiciona serviços da aplicação
builder.Services.AddServices();

// Adiciona serviços do banco de dados
builder.Services.AddDb(builder.Configuration);

var app = builder.Build();

/* ----------------------- Configuração de Middlewares ---------------------- */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "openapi/{documentName}.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Produto API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
