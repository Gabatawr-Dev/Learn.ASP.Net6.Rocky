using Rocky.Models.Product;

namespace Rocky.Models.Cart;

public class CartSummaryModel
{
    public AppUserDTO AppAppUser { get; set; }
    public IList<CartIndexModel>? ItemCollection { get; set; }

    public CartSummaryModel() => AppAppUser = new AppUserDTO();
}

