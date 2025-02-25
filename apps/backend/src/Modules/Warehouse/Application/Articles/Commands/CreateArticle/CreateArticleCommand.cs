using Application.Messaging.Commands;
using Warehouse.Domain.Articles.Request;

namespace Warehouse.Application.Articles.Commands.CreateArticle;

public sealed record CreateArticleCommand(CreateArticleRequest Request) : ICommand;
