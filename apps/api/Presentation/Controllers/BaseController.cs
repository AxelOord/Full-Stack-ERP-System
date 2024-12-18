using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Application.Generics.Create;
using Domain.Primitives;
using Application.Generics.Delete;
using Api.Infrastructure;
using Domain.Specifications;
using Domain.Shared.ApiResponse;
using Application.Generics.GetAll;
using Application.Generics.GetById;
using Api.Extensions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TDto>(IServiceFactory serviceFactory, IMapper mapper, LinkBuilder linkBuilder) : ControllerBase
        where TEntity : Entity, new()
        where TDto : Dto
    {
        private readonly IGetByIdService<TEntity> _getByIdService = serviceFactory.GetGetByIdService<TEntity>();
        private readonly IGetAllService<TEntity> _getAllService = serviceFactory.GetGetAllService<TEntity>();
        private readonly ICreateService<TDto, TEntity> _createService = serviceFactory.GetCreateService<TDto, TEntity>();
        private readonly IDeleteService<TEntity> _deleteService = serviceFactory.GetDeleteService<TEntity>();
        private readonly IMapper _mapper = mapper;
        private readonly LinkBuilder _linkBuilder = linkBuilder;

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
        public async Task<ActionResult<Response<TDto>>> GetAllAsync([FromQuery] GetAllQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var specification = new GetAllEntitiesSpecification<TEntity>(queryParameters);

            var query = new GetAllQuery<TEntity>(specification);
            var result = await _getAllService.ExecuteAsync(query, cancellationToken);
            var mapperStrategy = new CollectionEntityMapper<TEntity, TDto>();

            return result.ToActionResult(_mapper, mapperStrategy, _linkBuilder, Request);
        }

        /// <summary>
        /// Create entity
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] TDto? dto, CancellationToken cancellationToken)
        {
            var command = new CreateCommand<TDto, TEntity>(dto!);
            var result = await _createService.ExecuteAsync(command, cancellationToken);

            return result.ToActionResult(StatusCodes.Status201Created);
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
