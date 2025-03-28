using FinTrack.Application.UseCases.Products.RegisterProduct;
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
}