using Microsoft.AspNetCore.Mvc;
using Produto.Application;
using Produto.Application.Dtos;
using Produto.Application.Services;

namespace Produto.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Tags("Produtos")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutosService _service;

    public ProdutosController(ProdutosService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os produtos
    /// </summary>
    /// <param name="page">Número da página. Padrão: 1</param>
    /// <param name="pageSize">Número de itens por página. Padrão: 10, Máximo: 100</param>
    /// <param name="search">Termo para busca no nome do produto. Máximo: 50 caracteres</param>
    /// <param name="sortBy">Campos para ordenação. Formato: campo:asc|desc,campo2:asc|desc. Campos permitidos: id, nome, descricao, valor, estoque</param>
    [HttpGet]
    [ProducesResponseType(typeof(AppPagedResult<ProdutoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;
        if (pageSize > 100) pageSize = 100;
        if (string.IsNullOrEmpty(search)) search = null;
        else if (search.Length > 50) search = search[..50];

        var sortDict = sortBy.ParseSortBy();
        var result = await _service.GetAsync(page, pageSize, search, sortDict);
        return Ok(result);
    }

    /// <summary>
    /// Retorna um produto pelo ID ou nome
    /// </summary>
    [HttpGet("{idOuNome}")]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] string idOuNome)
    {
        var result = await _service.GetByIdAsync(idOuNome);
        return result.StatusCode switch
        {
            AppResultStatus.Sucesso => Ok(result),
            AppResultStatus.NaoEncontrado => NotFound(result),
            _ => BadRequest(result)
        };
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] ProdutoDto produtoDto)
    {
        var result = await _service.CreateAsync(produtoDto);
        return result.StatusCode switch
        {
            AppResultStatus.Sucesso => CreatedAtAction(nameof(GetById), new { idOuNome = result.Data?.Id }, result),
            _ => BadRequest(result)
        };
    }

    /// <summary>
    /// Atualiza um produto
    /// </summary>
    [HttpPut("{idOuNome}")]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(AppResult<ProdutoDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromRoute] string idOuNome, [FromBody] ProdutoDto produtoDto)
    {
        if (string.IsNullOrEmpty(idOuNome))
        {
            return BadRequest(new AppResult<ProdutoDto>(
                message: "ID do produto não pode ser vazio",
                statusCode: AppResultStatus.Erro,
                success: false));
        }

        var result = await _service.UpdateAsync(idOuNome, produtoDto);
        return result.StatusCode switch
        {
            AppResultStatus.Sucesso => Ok(result),
            AppResultStatus.NaoEncontrado => NotFound(result),
            _ => BadRequest(result)
        };
    }

    /// <summary>
    /// Deleta um produto
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(AppResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(AppResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result.StatusCode switch
        {
            AppResultStatus.Sucesso => NoContent(),
            AppResultStatus.NaoEncontrado => NotFound(result),
            _ => BadRequest(result)
        };
    }
} 