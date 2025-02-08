using Application.Messaging.Commands;
using Warehouse.Domain.Articles.Dtos;

namespace Warehouse.Application.Articles.Commands.CreateArticle
{
  public sealed record CreateArticleCommand(CreateArticleDto Dto) : ICommand;
}
