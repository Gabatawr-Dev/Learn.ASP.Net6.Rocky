using System.ComponentModel.DataAnnotations;
using Rocky.Models.Product;

namespace Rocky.Models.Home;

public class HomeDetailsModel
{
    public ProductDTO Product { get; set; }
    public bool IsExistInCard { get; set; }

    [Range(1, 10_000)]
    public uint TempSqFt { get; set; }

    public HomeDetailsModel()
    {
        Product = new ProductDTO();
        TempSqFt = 1;
    }
}