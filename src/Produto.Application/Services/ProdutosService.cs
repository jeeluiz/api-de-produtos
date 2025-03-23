namespace Produto.Application.Services;

using FluentValidation;

using Microsoft.Extensions.Logging;

using Produto.Application.Dtos;
using Produto.Data.Entities;
using Produto.Data.Repositories;

/// <summary>
/// Serviço responsável por gerenciar as operações relacionadas a produtos.
/// Implementa a lógica de negócio e validações para manipulação de produtos.
/// </summary>
public class ProdutosService
{
    private readonly IProdutosRepository _repository;
    private readonly ILogger _logger;
    private readonly IValidator<ProdutoDto> _produtoValidator;

    /// <summary>
    /// Inicializa uma nova instância do serviço de produtos.
    /// </summary>
    /// <param name="repository">Repositório de produtos</param>
    /// <param name="logger">Logger para registro de eventos</param>
    /// <param name="produtoValidator">Validador de produtos</param>
    public ProdutosService(IProdutosRepository repository, ILogger<ProdutosService> logger, IValidator<ProdutoDto> produtoValidator)
    {
        _logger = logger;
        _repository = repository;
        _produtoValidator = produtoValidator;
    }

    /// <summary>
    /// Obtém uma lista paginada de produtos com suporte a busca e ordenação.
    /// </summary>
    /// <param name="page">Número da página desejada (começa em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <param name="search">Termo de busca para filtrar produtos pelo nome</param>
    /// <param name="sortDict">Dicionário de ordenação no formato {campo, direção}</param>
    /// <returns>
    /// Retorna um AppPagedResult contendo:
    /// - Lista de produtos paginada
    /// - Total de registros
    /// - Informações de paginação
    /// Em caso de erro, retorna mensagem apropriada
    /// </returns>
    public async Task<AppPagedResult<ProdutoDto>> GetAsync(int? page = null,
                                                           int? pageSize = null,
                                                           string? search = null,
                                                           IReadOnlyDictionary<string, string>? sortDict = null
        )
    {
        try {
            var produtos = await _repository.GetAsync(page, pageSize, search, sortDict);
            var totalCount = await _repository.GetCountAsync(search);

            var result = new AppPagedResult<ProdutoDto>(
                data: produtos.Select(ProdutoDto.FromEntity).ToList(),
                totalCount: totalCount,
                page: page ?? 1,
                pageSize: pageSize ?? produtos.Count
            );

            return result;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro ao buscar produtos");
            return new AppPagedResult<ProdutoDto>("Erro ao buscar produtos", false, AppResultStatus.Erro);
        }
    }

    /// <summary>
    /// Busca um produto pelo ID ou nome.
    /// </summary>
    /// <param name="idOuNome">ID (GUID) ou nome do produto</param>
    /// <returns>
    /// Retorna um AppResult contendo:
    /// - Sucesso: Dados do produto encontrado
    /// - Não encontrado: Mensagem indicando que o produto não existe
    /// - Erro: Mensagem de erro em caso de falha
    /// </returns>
    public async Task<AppResult<ProdutoDto>> GetByIdAsync(string idOuNome)
    {
        try {

            ProdutoEntity? produto;
            if (Guid.TryParse(idOuNome, out var id)) {
                produto = await _repository.GetByIdAsync(id);
            }
            else {
                produto = await _repository.GetByNomeAsync(idOuNome.Trim());
            }

            if (produto == null) {
                return new AppResult<ProdutoDto>("Produto não encontrado", false, AppResultStatus.NaoEncontrado);
            }

            return new AppResult<ProdutoDto>(ProdutoDto.FromEntity(produto));
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro ao buscar produto");
            return new AppResult<ProdutoDto>("Erro ao buscar produto", false, AppResultStatus.Erro);
        }
    }

    /// <summary>
    /// Cria um novo produto no sistema.
    /// </summary>
    /// <param name="produtoDto">Dados do produto a ser criado</param>
    /// <returns>
    /// Retorna um AppResult contendo:
    /// - Sucesso: Dados do produto criado
    /// - Erro de validação: Lista de erros de validação
    /// - Erro de duplicidade: Mensagem indicando que o produto já existe
    /// - Erro: Mensagem de erro em caso de falha
    /// </returns>
    public async Task<AppResult<ProdutoDto>> CreateAsync(ProdutoDto produtoDto)
    {
        try {
            var validationResult = await _produtoValidator.ValidateAsync(produtoDto);
            if (!validationResult.IsValid) {
                return validationResult.ToAppResult<ProdutoDto>();
            }
            var produtoExiste = await _repository.GetByNomeAsync(produtoDto.Nome.Trim());

            if (produtoExiste != null) {
                return new AppResult<ProdutoDto>("Um produto com esse nome já existe", false, AppResultStatus.Erro);
            }    
            
            var produto = new ProdutoEntity(produtoDto.Nome.Trim(), produtoDto.Valor,produtoDto.Estoque, produtoDto.Descricao);
            await _repository.AddAsync(produto);
            return new AppResult<ProdutoDto>(ProdutoDto.FromEntity(produto));
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro ao criar produto");
            return new AppResult<ProdutoDto>("Erro ao criar produto", false, AppResultStatus.Erro);
        }
    }

    /// <summary>
    /// Atualiza um produto existente no sistema.
    /// </summary>
    /// <param name="idOuNome">ID (GUID) ou nome do produto a ser atualizado</param>
    /// <param name="produtoDto">Novos dados do produto</param>
    /// <returns>
    /// Retorna um AppResult contendo:
    /// - Sucesso: Dados do produto atualizado
    /// - Não encontrado: Mensagem indicando que o produto não existe
    /// - Erro de validação: Lista de erros de validação
    /// - Erro de duplicidade: Mensagem indicando que já existe outro produto com o nome informado
    /// - Erro: Mensagem de erro em caso de falha
    /// </returns>
    public async Task<AppResult<ProdutoDto>> UpdateAsync(string idOuNome, ProdutoDto produtoDto)
    {
        try {
            var validationResult = await _produtoValidator.ValidateAsync(produtoDto);
            if (!validationResult.IsValid) {
                return validationResult.ToAppResult<ProdutoDto>();
            }

            ProdutoEntity? produto;
            if(Guid.TryParse(idOuNome,out var id)) {
                produto = await _repository.GetByIdAsync(id);
            }
            else {
                produto = await _repository.GetByNomeAsync(idOuNome.Trim());
            }


            if (produto == null) {
                return new AppResult<ProdutoDto>("Produto não encontrado", false, AppResultStatus.NaoEncontrado);
            }
            var produtoExiste = await _repository.GetByNomeAsync(produtoDto.Nome.Trim());

            if (produtoExiste != null && produtoExiste.Id !=produto.Id) {
                return new AppResult<ProdutoDto>("Um produto com esse nome já existe", false, AppResultStatus.Erro);
            }

            produto.Nome = produtoDto.Nome.Trim();
            produto.Descricao = produtoDto.Descricao;
            produto.Valor = produtoDto.Valor;
            produto.Estoque = produtoDto.Estoque;

            await _repository.UpdateAsync(produto);
            return new AppResult<ProdutoDto>(ProdutoDto.FromEntity(produto));
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro ao atualizar produto");
            return new AppResult<ProdutoDto>("Erro ao atualizar produto", false, AppResultStatus.Erro);
        }
    }

    /// <summary>
    /// Remove um produto do sistema.
    /// </summary>
    /// <param name="id">ID (GUID) do produto a ser removido</param>
    /// <returns>
    /// Retorna um AppResult indicando:
    /// - Sucesso: Produto removido com sucesso
    /// - Não encontrado: Mensagem indicando que o produto não existe
    /// - Erro: Mensagem de erro em caso de falha
    /// </returns>
    public async Task<AppResult> DeleteAsync(Guid id)
    {
        try {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null) {
                return new AppResult("Produto não encontrado", false, AppResultStatus.NaoEncontrado);
            }

            await _repository.DeleteAsync(id);
            return new AppResult();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro ao deletar produto");
            return new AppResult("Erro ao deletar produto", false, AppResultStatus.Erro);
        }
    }
}