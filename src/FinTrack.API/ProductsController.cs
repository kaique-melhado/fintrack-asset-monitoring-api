using FinTrack.Application.UseCases.Products.Commands.RegisterProduct;
using FinTrack.Application.UseCases.Products.Queries.GetProductById;
using FinTrack.Application.UseCases.Products.Queries.Responses;
using FinTrack.Configuration.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers;

/// <summary>
/// Controlador responsável por gerenciar operações de produtos financeiros, como cadastro e consulta.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="ProductsController"/>.
    /// </summary>
    /// <param name="mediator">Instância do MediatR para orquestrar os comandos e consultas.</param>
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastra um novo produto no sistema.
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     POST /api/products
    ///     {
    ///         "name": "Banco PAN",
    ///         "ticker": "BPAN4",
    ///         "type": 1,
    ///         "category": 1,
    ///         "currencyCode": "BRL"
    ///     }
    ///     
    /// </remarks>
    /// <param name="command">Comando contendo os dados do produto.</param>
    /// <returns>Retorna os dados do produto cadastrado.</returns>
    /// <response code="201">Produto criado com sucesso.</response>
    /// <response code="400">Dados inválidos ou regras de negócio violadas.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RegisterProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterProduct([FromBody] RegisterProductCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(RegisterProduct), new { id = result.Id }, result);
    }

    /// <summary>
    /// Retorna os detalhes de um produto a partir do seu ID.
    /// </summary>
    /// <param name="id">Identificador do produto.</param>
    /// <returns>Produto encontrado ou erro 404 caso não exista.</returns>
    /// <response code="200">Produto encontrado com sucesso.</response>
    /// <response code="404">Produto não encontrado para o ID informado.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(result);
    }
}