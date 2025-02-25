
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation;
using Shared.Extensions;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Domain.Suppliers;
using Warehouse.Domain.Suppliers.Dto;
using Warehouse.Domain.Suppliers.Request;
using Warehouse.Persistence;

namespace Warehouse.Presentation.Controllers;

public class SuppliersController(IMapper mapper, IMediator mediator, Func<Type, Type, Type, object> factory)
: BaseController<Supplier, SupplierDto, WarehouseDbContext>(mapper, mediator, factory)
{

    /// <summary>
    /// Create supplier
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSupplierCommand(request);
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(StatusCodes.Status201Created);
    }
}
