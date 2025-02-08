using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Warehouse.Domain.Articles;
using Warehouse.Domain.Articles.Dtos;
using Microsoft.AspNetCore.Http;
using Shared.Results.Response;
using Shared.Extensions;
using Warehouse.Application.Articles.Commands.CreateArticle;
using Presentation;
using Warehouse.Persistence;

namespace Warehouse.Presentation.Controllers
{
  public class ArticlesController(IMapper mapper, LinkBuilder linkBuilder, IMediator mediator, Func<Type, Type, Type, object> factory)
     : BaseController<Article, ArticleDto, WarehouseDbContext>(mapper, linkBuilder, mediator, factory)
  {
    /// <summary>
    /// Create entity
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArticleDto? dto, CancellationToken cancellationToken)
    {
      var command = new CreateArticleCommand(dto!);
      var result = await _mediator.Send(command, cancellationToken);

      return result.ToActionResult(StatusCodes.Status201Created);
    }
  }
}
