using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models.Product;

public class ProductUpsertModel
{
    public ProductDTO? Product { get; set; }

    public IEnumerable<SelectListItem>? CategoryList { get; set; }
}

