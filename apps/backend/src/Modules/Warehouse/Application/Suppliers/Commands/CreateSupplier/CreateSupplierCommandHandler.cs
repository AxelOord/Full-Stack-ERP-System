using Application.Messaging.Commands;
using Shared.Results;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands.CreateSupplier
{
  public class CreateSupplierCommandHandler : ICommandHandler<CreateSupplierCommand>
  {
    private readonly ICreateSupplierService _createSupplierService;

    public CreateSupplierCommandHandler(ICreateSupplierService createArticleService)
    {
      _createSupplierService = createArticleService;
    }

    public async Task<Result> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
      var result = await _createSupplierService.ExecuteAsync(Supplier.Create(request.Dto), cancellationToken);

      return result;
    }
  }
}
