
using Microsoft.EntityFrameworkCore;

using Produto.Data.Entities;

namespace Produto.Data.Repositories;

internal class ProdutosRepository : IProdutosRepository
{
    private readonly ProdutosContext _dbContext;

    public ProdutosRepository(ProdutosContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetCountAsync(string? search = null)
    {
        var query = _dbContext.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(search)) {
            query = query.Where(p => p.Nome.Contains(search));
        }

        return await query.CountAsync();
    }

    /// <summary>
    /// Obtém uma lista de produtos com paginação e pesquisa.
    /// </summary>
    /// <param name="page">página para a qual deseja obter os produtos</param>
    /// <param name="pageSize">tamanho da página</param>
    /// <param name="search">termo de pesquisa para filtrar os produtos</param>
    /// <returns>lista de produtos</returns>
    public async Task<IReadOnlyCollection<ProdutoEntity>> GetAsync(
        int? page = null,
        int? pageSize = null,
        string? search = null,
        IReadOnlyDictionary<string, string>? sortDict = null)
    {
        var query = _dbContext.Produtos.AsQueryable();

        if (!string.IsNullOrEmpty(search)) {
            query = query.Where(p => p.Nome.Contains(search));
        }

        if (sortDict != null) {
            IOrderedQueryable<ProdutoEntity>? orderedQuery = null;
           
            foreach (var kvp in sortDict) {
                switch (kvp.Key.ToLower()) {
                    case "id":
                        if (kvp.Value == "asc") {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderBy(p => p.Id);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenBy(p => p.Id);
                            }
                        }
                        else {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderByDescending(p => p.Id);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenByDescending(p => p.Id);
                            }
                        }
                        break;

                    case "nome":
                        if (kvp.Value == "asc") {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderBy(p => p.Nome);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenBy(p => p.Nome);
                            }
                        }
                        else {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderByDescending(p => p.Nome);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenByDescending(p => p.Nome);
                            }
                        }
                        break;

                    case "descricao":
                        if (kvp.Value == "asc") {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderBy(p => p.Descricao);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenBy(p => p.Descricao);
                            }
                        }
                        else {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderByDescending(p => p.Descricao);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenByDescending(p => p.Descricao);
                            }
                        }
                        break;
                    case "valor":
                        if (kvp.Value == "asc") {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderBy(p => p.Valor);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenBy(p => p.Valor);
                            }
                        }
                        else {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderByDescending(p => p.Valor);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenByDescending(p => p.Valor);
                            }
                        }
                        break;

                    case "estoque":
                        if (kvp.Value == "asc") {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderBy(p => p.Estoque);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenBy(p => p.Estoque);
                            }
                        }
                        else {
                            if (orderedQuery == null) {
                                orderedQuery = query.OrderByDescending(p => p.Estoque);
                            }
                            else {
                                orderedQuery = orderedQuery.ThenByDescending(p => p.Estoque);
                            }
                        }
                        break;
                }
                if(orderedQuery != null) {
                    query = orderedQuery;
                }
            }
        }

        if (page.HasValue && pageSize.HasValue) {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }

    /// <summary>
    /// Obtém um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto</param>
    /// <returns>Produto encontrado ou null se não encontrado</returns>
    public async Task<ProdutoEntity?> GetByIdAsync(Guid id)
    {
        var produto = await _dbContext.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return produto;
    }

    public async Task<ProdutoEntity?> GetByNomeAsync(string nome)
    {
        var produto = await _dbContext.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nome == nome);

        return produto;
    }
    public async Task AddAsync(ProdutoEntity produto)
    {
        await _dbContext.Produtos.AddAsync(produto);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProdutoEntity produto)
    {
        var existingProduto = await GetByIdAsync(produto.Id);
        if (existingProduto != null) {
            _dbContext.Entry(existingProduto).CurrentValues.SetValues(produto);
            _dbContext.Update(existingProduto);
            await _dbContext.SaveChangesAsync();
        }
        else {
            throw new KeyNotFoundException($"Produto com ID {produto.Id} não encontrado.");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var produto = await GetByIdAsync(id);
        if (produto != null) {
            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync();
        }
        else {
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
        }
    }
}
