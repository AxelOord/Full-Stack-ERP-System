using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation;
using Shared.Extensions;
using Warehouse.Application.Articles.Commands.CreateArticle;
using Warehouse.Application.Articles.Commands.CreateArticleVariant;
using Warehouse.Domain.Articles;
using Warehouse.Domain.Articles.Dto;
using Warehouse.Domain.Articles.Request;
using Warehouse.Persistence;

namespace Warehouse.Presentation.Controllers;

public class ArticlesController(IMapper mapper, IMediator mediator, Func<Type, Type, Type, object> factory)
 : BaseController<Article, ArticleDto, WarehouseDbContext>(mapper, mediator, factory)
{
    /// <summary>
    /// Create supplier
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArticleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateArticleCommand(request);
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Add a variant to an article
    /// </summary>
    [HttpPost("{id:guid}/variants")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddVariantAsync(Guid id, [FromBody] CreateArticleVariantRequest request, CancellationToken cancellationToken)
    {
        if (id != request.ArticleId)
            return BadRequest(new ProblemDetails
            {
                Title = "Mismatched article ID",
                Detail = "The article ID in the URL does not match the ID in the request body."
            });

        var command = new CreateArticleVariantCommand(request);
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(StatusCodes.Status201Created);
    }
}
