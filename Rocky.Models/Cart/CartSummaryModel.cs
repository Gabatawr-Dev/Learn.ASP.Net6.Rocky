using Rocky.Models.Product;

namespace Rocky.Models.Cart;

public class CartSummaryModel
{
    public AppUserDTO AppAppUser { get; set; }
    public IList<ProductDTO>? Products { get; set; }

    public CartSummaryModel() => AppAppUser = new AppUserDTO();
}

