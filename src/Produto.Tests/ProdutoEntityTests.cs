using System;
using Xunit;
using Produto.Data.Entities;

namespace Produto.Tests;
public class ProdutoEntityTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var nome = "Produto Teste";
        var valor = 100.50m;
        var estoque = 10;
        var descricao = "Descrição do produto";

        // Act
        var produto = new ProdutoEntity(nome, valor, estoque, descricao);

        // Assert
        Assert.Equal(nome, produto.Nome);
        Assert.Equal(valor, produto.Valor);
        Assert.Equal(estoque, produto.Estoque);
        Assert.Equal(descricao, produto.Descricao);
        Assert.NotEqual(Guid.Empty, produto.Id);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNomeIsNullOrWhiteSpace()
    {
        Assert.Throws<ArgumentNullException>(() => new ProdutoEntity("", 10m, 5));
        Assert.Throws<ArgumentNullException>(() => new ProdutoEntity(" ", 10m, 5));
        Assert.Throws<ArgumentNullException>(() => new ProdutoEntity(null!, 10m, 5));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenValorIsZeroOrNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ProdutoEntity("Produto", 0m, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ProdutoEntity("Produto", -10m, 5));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenEstoqueIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ProdutoEntity("Produto", 10m, -5));
    }

    [Fact]
    public void SetNome_ShouldThrowException_WhenValueIsNullOrWhiteSpace()
    {
        var produto = new ProdutoEntity("Produto", 10m, 5);
        Assert.Throws<ArgumentNullException>(() => produto.Nome = "");
        Assert.Throws<ArgumentNullException>(() => produto.Nome = " ");
        Assert.Throws<ArgumentNullException>(() => produto.Nome = null!);
    }

    [Fact]
    public void SetValor_ShouldThrowException_WhenValueIsZeroOrNegative()
    {
        var produto = new ProdutoEntity("Produto", 10m, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => produto.Valor = 0m);
        Assert.Throws<ArgumentOutOfRangeException>(() => produto.Valor = -10m);
    }

    [Fact]
    public void SetEstoque_ShouldThrowException_WhenValueIsNegative()
    {
        var produto = new ProdutoEntity("Produto", 10m, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => produto.Estoque = -1);
    }
}
