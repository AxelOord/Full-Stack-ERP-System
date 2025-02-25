using Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Suppliers;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Persistence.Suppliers;

public class SupplierRepository : ISupplierRepository, IScoped
{
    private readonly WarehouseDbContext _context;

    public SupplierRepository(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Supplier>()
                  .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
