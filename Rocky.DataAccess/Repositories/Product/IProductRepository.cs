using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Models.Product;

namespace Rocky.DataAccess.Repositories.Product;

public interface IProductRepository : IRepository<ProductDTO>
{
    public IEnumerable<SelectListItem> GetCategoryDropDown();
}
