using Application.Warehouse.Suppliers;
using Domain.Organizations.Suppliers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Persistence.Suppliers
{
  public class SupplierRepository : ISupplierRepository
  {
    private readonly ApplicationDbContext _context;

    public SupplierRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
  }
}
