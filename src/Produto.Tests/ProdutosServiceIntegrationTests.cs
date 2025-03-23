

using FluentValidation;

using Microsoft.Extensions.Logging;

using Moq;

using Produto.Application.Dtos;
using Produto.Application.Services;
using Produto.Data.Entities;
using Produto.Data.Repositories;

using Xunit;
namespace Produto.Tests;

public class ProdutosServiceIntegrationTests
{
    private readonly ProdutosService _service;
    private readonly Mock<IProdutosRepository> _repositoryMock;
    private readonly Mock<ILogger<ProdutosService>> _loggerMock;
    private readonly Mock<IValidator<ProdutoDto>> _validatorMock;

    public ProdutosServiceIntegrationTests()
    {
        _repositoryMock = new Mock<IProdutosRepository>();
        _loggerMock = new Mock<ILogger<ProdutosService>>();
        _validatorMock = new Mock<IValidator<ProdutoDto>>();

        _service = new ProdutosService(_repositoryMock.Object, _loggerMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarProdutoComSucesso()
    {
        var produtoDto = new ProdutoDto { Nome = "Produto Teste", Valor = 100, Estoque = 10, Descricao = "Teste" };
        var produtoEntity = new ProdutoEntity(produtoDto.Nome, produtoDto.Valor, produtoDto.Estoque, produtoDto.Descricao);

        _validatorMock.Setup(v => v.ValidateAsync(produtoDto, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _repositoryMock.Setup(r => r.GetByNomeAsync(produtoDto.Nome))
                       .ReturnsAsync((ProdutoEntity)null);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<ProdutoEntity>()))
                       .Returns(Task.CompletedTask);

        var resultado = await _service.CreateAsync(produtoDto);

        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Data);
        Assert.Equal(produtoDto.Nome, resultado.Data.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProduto_QuandoIdForValido()
    {
        var id = Guid.NewGuid();
        var produto = new ProdutoEntity("Produto Teste", 100, 10, "Teste") { Id = id };

        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);

        var resultado = await _service.GetByIdAsync(id.ToString());

        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Data);
        Assert.Equal(id, resultado.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarProdutoComSucesso()
    {
        var id = Guid.NewGuid();
        var produto = new ProdutoEntity("Produto Antigo", 50, 5, "Antigo") { Id = id };
        var produtoDto = new ProdutoDto { Nome = "Produto Novo", Valor = 150, Estoque = 20, Descricao = "Novo" };

        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);
        _repositoryMock.Setup(r => r.GetByNomeAsync(produtoDto.Nome)).ReturnsAsync((ProdutoEntity)null);
        _validatorMock.Setup(v => v.ValidateAsync(produtoDto, default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var resultado = await _service.UpdateAsync(id.ToString(), produtoDto);

        Assert.True(resultado.Success);
        Assert.Equal(produtoDto.Nome, resultado.Data.Nome);
    }

    [Fact]
    public async Task DeleteAsync_DeveDeletarProdutoComSucesso()
    {
        var id = Guid.NewGuid();
        var produto = new ProdutoEntity("Produto Teste", 100, 10, "Teste") { Id = id };

        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);
        _repositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var resultado = await _service.DeleteAsync(id);

        Assert.True(resultado.Success);
    }
}
