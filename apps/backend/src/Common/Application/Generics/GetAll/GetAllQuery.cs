using Application.Messaging.Queries;
using Domain.Primitives.Interfaces;
using Shared.Specifications;

namespace Application.Generics.GetAll;

public sealed record GetAllQuery<T>(GetAllEntitiesSpecification<T> Specification) : IQuery<List<T>> where T : IEntity;
