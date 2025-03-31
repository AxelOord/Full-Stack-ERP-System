using Application.Messaging.Commands;
using Warehouse.Domain.Articles.Request;

namespace Warehouse.Application.Articles.Commands.CreateArticleVariant;

public sealed record CreateArticleVariantCommand(CreateArticleVariantRequest Request) : ICommand;
