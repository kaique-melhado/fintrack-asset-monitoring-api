using FinTrack.Application.UseCases.Products.Commands.RegisterProduct;
using FinTrack.Application.UseCases.Products.Queries.GetProductById;
using FinTrack.Application.UseCases.Products.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers;

/// <summary>
/// Controlador responsável por gerenciar operações relacionadas a produtos.
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
    /// Registra um novo produto no sistema.
    /// </summary>
    /// <param name="command">Comando contendo os dados do produto.</param>
    /// <returns>Retorna os dados do produto registrado.</returns>
    /// <response code="201">Produto criado com sucesso.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RegisterProductResponse), StatusCodes.Status201Created)]
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
    /// <response code="200">Produto encontrado.</response>
    /// <response code="404">Produto não encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(result);
    }

}