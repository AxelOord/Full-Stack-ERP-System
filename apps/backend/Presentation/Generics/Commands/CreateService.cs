using Application.Generics.Create;
using AutoMapper;
using Domain.Primitives;
using Domain.Shared;

namespace Persistence.Generics.Commands
{
    public sealed class CreateService<TDto, TEntity> : ICreateService<TDto, TEntity>
        where TDto : Dto
        where TEntity : Entity, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> ExecuteAsync(CreateCommand<TDto, TEntity> request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(request.Dto);

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
