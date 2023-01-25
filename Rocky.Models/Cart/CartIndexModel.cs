using Rocky.Models.Product;

namespace Rocky.Models.Cart;

public class CartIndexModel
{
    public ProductDTO? Product { get; set; }
    public uint SqFt { get; set; }
}