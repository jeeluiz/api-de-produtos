namespace Produto.Data.Entities;

using System.Diagnostics.CodeAnalysis;

public class ProdutoEntity : IEntity
{
    private string _nome;
    private decimal _valor;
    private int _estoque;
    public ProdutoEntity(string nome, decimal valor, int estoque, string? descricao = null)
    {
        Id = Guid.CreateVersion7();
        Nome = nome;
        Valor = valor;
        Descricao = descricao;
        Estoque = estoque;
    }

    public ProdutoEntity(Guid id, string nome, decimal valor, int estoque, string? descricao = null)
    {
        Id = id;
        Nome = nome;
        Valor = valor;
        Descricao = descricao;
        Estoque = estoque;
    }

    // Construtor para o EF
#pragma warning disable CS8618
    protected ProdutoEntity() { }
#pragma warning restore CS8618 

    public Guid Id { get; set; }
    public string Nome {
        get => _nome;
        [MemberNotNull(nameof(_nome))]
        set => _nome = string.IsNullOrWhiteSpace(value)
                     ? throw new ArgumentNullException(nameof(Nome), "Nome não pode ser vazio")
                     : value;
    }
    public string? Descricao { get; set; }
    public decimal Valor {
        get => _valor;
        set => _valor = value <= 0
                      ? throw new ArgumentOutOfRangeException(nameof(Valor), "Valor deve ser maior que zero")
                      : value;
    }
    public int Estoque {
        get => _estoque;
        set => _estoque = value < 0
                      ? throw new ArgumentOutOfRangeException(nameof(Estoque), "Estoque não pode ser negativo")
                      : value;
    }
}
