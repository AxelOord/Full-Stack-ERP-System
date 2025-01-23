using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Application.Generics.Delete;
using Domain.Specifications;
using Domain.Shared.ApiResponse;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Api.Extensions;
using Infrastructure;
using Domain.Primitives.Interfaces;
using MediatR;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TDto>(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder, IMediator mediator) : ControllerBase
        where TEntity : class, IEntity, ICreatableFromDto<TEntity, TDto>
        where TDto : IDto
    {
        internal readonly IMediator _mediator = mediator;
        internal readonly IMapper _mapper = mapper;
        internal readonly LinkBuilder _linkBuilder = linkBuilder; // no Interface defuq?

        private readonly IGetByIdService<TEntity> _getByIdService = serviceFactory.GetGetByIdService<TEntity>();
        private readonly IGetAllService<TEntity> _getAllService = serviceFactory.GetGetAllService<TEntity>();
        private readonly IDeleteService<TEntity> _deleteService = serviceFactory.GetDeleteService<TEntity>();

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
