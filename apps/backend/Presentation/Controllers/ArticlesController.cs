using AutoMapper;
using Domain.Shared.ApiResponse;
using Infrastructure;
using Domain.Warehouse.Article;
using Domain.Warehouse.Article.Dto;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Api.Extensions;
using Application.Warehouse.Articles.Commands.CreateArticle;

namespace Presentation.Controllers
{
  public class ArticlesController(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder, IMediator mediator)
     : BaseController<Article, ArticleDto>(serviceFactory, mapper, linkBuilder, mediator)
    {    
      /// <summary>
      /// Create entity
      /// </summary>
      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
      public async Task<IActionResult> CreateAsync([FromBody] ArticleDto? dto, Guid supplierId, CancellationToken cancellationToken)
      {
        var command = new CreateArticleCommand(dto!, supplierId);
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(StatusCodes.Status201Created);
      }
  }
}
