
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation;
using Shared.Results.Response;
using Shared.Extensions;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Domain.Suppliers;
using Warehouse.Domain.Suppliers.Dtos;
using Warehouse.Persistence;

namespace Warehouse.Presentation.Controllers
{
  public class SuppliersController(IMapper mapper, LinkBuilder linkBuilder, IMediator mediator, Func<Type, Type, Type, object> factory)
    : BaseController<Supplier, SupplierDto, WarehouseDbContext>(mapper, linkBuilder, mediator, factory)
  {

    /// <summary>
    /// Create entity
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSupplierDto? dto, CancellationToken cancellationToken)
    {
      var command = new CreateSupplierCommand(dto!);
      var result = await _mediator.Send(command, cancellationToken);

      return result.ToActionResult(StatusCodes.Status201Created);
    }
  }
}
