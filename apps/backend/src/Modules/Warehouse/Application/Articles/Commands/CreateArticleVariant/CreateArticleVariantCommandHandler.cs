using Application.Messaging.Commands;
using Shared.Results;
using Warehouse.Domain.Articles;

namespace Warehouse.Application.Articles.Commands.CreateArticleVariant;

internal sealed class CreateArticleVariantCommandHandler : ICommandHandler<CreateArticleVariantCommand>
{
    private readonly ICreateArticleVariantService _createVariantService;
    private readonly IArticleRepository _articleRepository;

    public CreateArticleVariantCommandHandler(
        ICreateArticleVariantService createVariantService,
        IArticleRepository articleRepository)
    {
        _createVariantService = createVariantService;
        _articleRepository = articleRepository;
    }

    public async Task<Result> Handle(CreateArticleVariantCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(command.Request.ArticleId, cancellationToken);
        if (article is null)
        {
            return Result.Failure(new NotFoundError(nameof(Article)));
        }

        return await _createVariantService.ExecuteAsync(ArticleVariant.Create(command.Request, article), cancellationToken);
    }
}
