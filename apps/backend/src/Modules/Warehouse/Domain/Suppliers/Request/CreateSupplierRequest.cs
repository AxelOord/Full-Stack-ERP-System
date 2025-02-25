using System.ComponentModel.DataAnnotations;

namespace Warehouse.Domain.Suppliers.Request;

public class CreateSupplierRequest
{
    [Required]
    public required int SbnId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public bool IsActive { get; set; }
}
