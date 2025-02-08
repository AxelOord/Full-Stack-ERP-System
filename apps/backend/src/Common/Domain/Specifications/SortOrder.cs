using System.Linq.Expressions;

namespace Domain.Specifications
{
  public class SortOrder<T>
  {
    public Expression<Func<T, object>> KeySelector { get; set; }
    public bool IsDescending { get; set; }
  }
}
