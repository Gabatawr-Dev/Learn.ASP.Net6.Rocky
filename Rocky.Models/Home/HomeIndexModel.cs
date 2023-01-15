using Rocky.Models.Category;
using Rocky.Models.Product;

namespace Rocky.Models.Home;

public class HomeIndexModel
{
    public IEnumerable<ProductDTO>? Products { get; set; }
    public IEnumerable<CategoryDTO>? Categories { get; set; }
}

