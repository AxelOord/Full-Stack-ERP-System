using Application.Generics.Messaging.Commands;
using Domain.Primitives.Interfaces;
using Domain.Warehouse.Article.Dto;

namespace Application.Warehouse.Articles.Commands.CreateArticle
{
  public sealed record CreateArticleCommand(ArticleDto Dto, Guid SupplierId) : ICommand;
}
