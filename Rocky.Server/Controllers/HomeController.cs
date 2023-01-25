using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Rocky.DataAccess.Repositories.Category;
using Rocky.DataAccess.Repositories.Product;
using Rocky.Models;
using Rocky.Models.Home;
using Rocky.Models.Product;
using Rocky.Utility;

namespace Rocky.Server.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _repoProduct;
    private readonly ICategoryRepository _repoCategory;

    public HomeController(
        ILogger<HomeController> logger,
        IProductRepository repoProduct,
        ICategoryRepository repoCategory)
    {
        _logger = logger;
        _repoProduct = repoProduct;
        _repoCategory = repoCategory;
    }

    #region Index

    public IActionResult Index()
    {
        var model = new HomeIndexModel()
        {
            Products = _repoProduct.GetAll(include: nameof(ProductDTO.Category)),
            Categories = _repoCategory.GetAll()
        };
        return View(model);
    }

    #endregion Index

    #region Details

    public IActionResult Details(int id)
    {
        var product = _repoProduct
            .FirstOrDefault(p => p.Id == id, include: nameof(ProductDTO.Category));
        if (product == null)
            return NotFound();

        var model = new HomeDetailsModel
        {
            Product = product,
            IsExistInCard = GetSessionCart
                ?.Any(p => p.ProductId == id) ?? false,
        };
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult AddToCart(HomeDetailsModel model)
    {
        var sessionCart = GetSessionCart
            ?.ToList() ?? new List<ShoppingCart>();
        sessionCart.Add(new ShoppingCart
        {
            ProductId = model.Product.Id,
            SqFt = model.TempSqFt
        });
        HttpContext.Session.Set(Const.SessionCartList, sessionCart);

        TempData[Const.Success] = "Item successfully added to cart";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult RemoveFromCart(int id)
    {
        var sessionCart = GetSessionCart;
        var product = sessionCart
            ?.FirstOrDefault(p => p.ProductId == id);

        if (product != null)
        {
            sessionCart!.Remove(product);
            HttpContext.Session.Set(Const.SessionCartList, sessionCart);
        }

        TempData[Const.Success] = "Item successfully removed from cart";
        return RedirectToAction(nameof(Index));
    }

    #region DetailsHelpers

    private List<ShoppingCart>? GetSessionCart =>
        HttpContext.Session.Get<List<ShoppingCart>>(Const.SessionCartList);

    #endregion DetailsHelpers

    #endregion Details

    #region Error

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var model = new HomeErrorModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View(model);
    }

    #endregion Error
}