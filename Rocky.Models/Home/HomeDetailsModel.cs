using Rocky.Models.Product;

namespace Rocky.Models.Home;

public class HomeDetailsModel
{
    public ProductDTO Product { get; set; }
    public bool IsExistInCard { get; set; }

    public HomeDetailsModel() => Product = new ProductDTO();
}