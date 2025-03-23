namespace Produto.Application.Dtos;

using System.Text.Json.Serialization;

using Produto.Data.Entities;

public class ProdutoDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public int Estoque { get; set; }

    internal static ProdutoDto FromEntity(ProdutoEntity entity)
    {
        return new ProdutoDto {
            Id = entity.Id,
            Nome = entity.Nome,
            Descricao = entity.Descricao,
            Valor = entity.Valor,
            Estoque = entity.Estoque
        };
    }
}