using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Application.Generics.Delete;
using Domain.Specifications;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Domain.Primitives.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results.Response;
using Shared.Results.PaginatedResponse;
using Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Presentation
{
  [ApiController]
  [Route("api/[controller]")]
  public abstract class BaseController<TEntity, TDto, TDbContext>(IMapper mapper,
                           LinkBuilder linkBuilder,
                           IMediator mediator,
                           Func<Type, Type, Type, object> factory) : ControllerBase
    where TDbContext: DbContext
    where TEntity : class, IEntity
    where TDto : IDto
  {
    protected readonly IMediator _mediator = mediator;
    protected readonly IMapper _mapper = mapper;
    protected readonly LinkBuilder _linkBuilder = linkBuilder;

    private readonly Func<Type, Type, Type, object> _factory = factory;

    protected IGetByIdService<TEntity> _getByIdService =>
        (IGetByIdService<TEntity>)_factory(typeof(TEntity), typeof(IGetByIdService<>), typeof(TDbContext));

    protected IGetAllService<TEntity> _getAllService =>
        (IGetAllService<TEntity>)_factory(typeof(TEntity), typeof(IGetAllService<>), typeof(TDbContext));

    protected IDeleteService<TEntity> _deleteService =>
        (IDeleteService<TEntity>)_factory(typeof(TEntity), typeof(IDeleteService<>), typeof(TDbContext));

    /// <summary>
    /// Get entity by  id
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<TDto>>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      var query = new GetByIdQuery<TEntity>(id);
      var result = await _getByIdService.ExecuteAsync(query, cancellationToken);
      var mapperStrategy = new SingleEntityMapper<TEntity, TDto>();

      return result.ToActionResult(_mapper, mapperStrategy, _linkBuilder, Request);
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResponse<TDto>>> GetAllAsync([FromQuery] GetAllQueryParameters queryParameters, CancellationToken cancellationToken)
    {
      var specification = new GetAllEntitiesSpecification<TEntity>(queryParameters);

      var query = new GetAllQuery<TEntity>(specification);
      var result = await _getAllService.ExecuteAsync(query, cancellationToken);
      var mapperStrategy = new CollectionEntityMapper<TEntity, TDto>();

      return result.ToActionResult(_mapper, mapperStrategy, _linkBuilder, Request);
    }

    /// <summary>
    /// Delete entity by id
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      var command = new DeleteCommand<TEntity>(id);
      var result = await _deleteService.ExecuteAsync(command, cancellationToken);

      return result.ToActionResult();
    }
  }
}
