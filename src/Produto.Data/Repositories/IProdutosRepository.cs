namespace Produto.Data.Repositories;

using Produto.Data.Entities;

public interface IProdutosRepository
{
    Task<int> GetCountAsync(string? search = null);
    Task<IReadOnlyCollection<ProdutoEntity>> GetAsync(int? page = null, int? pageSize = null, string? search = null, IReadOnlyDictionary<string, string>? sortDict = null);
    Task<ProdutoEntity?> GetByIdAsync(Guid id);
    Task AddAsync(ProdutoEntity produto);
    Task UpdateAsync(ProdutoEntity produto);
    Task DeleteAsync(Guid id);
    Task<ProdutoEntity?> GetByNomeAsync(string nome);
}
