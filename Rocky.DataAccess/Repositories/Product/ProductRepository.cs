using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.DataAccess.Contexts;
using Rocky.Models.Product;

namespace Rocky.DataAccess.Repositories.Product;

public class ProductRepository : Repository<ProductDTO>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<SelectListItem> GetCategoryDropDown() =>
        Context.Category.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
}