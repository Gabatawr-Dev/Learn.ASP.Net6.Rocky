using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DataAccess.Repositories.AppUser;
using Rocky.DataAccess.Repositories.Inquiry;
using Rocky.DataAccess.Repositories.Product;
using Rocky.Models;
using Rocky.Models.Cart;
using Rocky.Models.Inquiry;
using Rocky.Utility;

namespace Rocky.Server.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly IAppUserRepository _repoAppUser;
    private readonly IProductRepository _repoProduct;
    private readonly IInquiryHeaderRepository _repoInquiryHeader;
    private readonly IInquiryDetailRepository _repoInquiryDetail;

    public CartController(
        ILogger<CartController> logger,
        IAppUserRepository repoAppUser,
        IProductRepository repoProduct,
        IInquiryHeaderRepository repoInquiryHeader,
        IInquiryDetailRepository repoInquiryDetail)
    {
        _logger = logger;
        _repoAppUser = repoAppUser;
        _repoProduct = repoProduct;
        _repoInquiryHeader = repoInquiryHeader;
        _repoInquiryDetail = repoInquiryDetail;
    }

    #region Index

    public IActionResult Index()
    {
        var sessionCart = GetSessionCart
            ?.ToList() ?? new();
        var model = new List<CartIndexModel>();

        foreach (var cart in sessionCart)
        {
            var product = _repoProduct.Find(cart.ProductId);
            if (product == null) continue;

            model.Add(new CartIndexModel
            {
                Product = product,
                SqFt = cart.SqFt
            });
        }
        
        return View(model);
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
            TempData[Const.Success] = "Item successfully removed from cart";

            if (sessionCart.Any() is false)
                return RedirectToAction("Index", "Home");
        }
        else if (product == null)
            TempData[Const.Error] = "Error removing item from cart";

        return RedirectToAction(nameof(Index));
    }

    #endregion Index

    #region Summary

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Summary(IList<CartIndexModel> modelIn)
    {
        var sessionCart = new List<ShoppingCart>();
        foreach (var item in modelIn)
        {
            var product = _repoProduct.Find(item.Product!.Id);
            if (product == null) continue;
            item.Product = product;

            var sqft = item.SqFt is > 0 and <= 10_000 ? item.SqFt : 1;
            item.SqFt = sqft;

            sessionCart.Add(new ShoppingCart
            {
                ProductId = product.Id,
                SqFt = sqft
            });
        }
        HttpContext.Session.Set(Const.SessionCartList, sessionCart);

        if (modelIn.Any() is false)
        {
            TempData[Const.Error] = "Order generation error";
            return RedirectToAction(nameof(Index));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var modelOut = new CartSummaryModel
        {
            AppAppUser = _repoAppUser.Find(userId) 
                         ?? new AppUserDTO(),
            ItemCollection = modelIn
        };

        return View(modelOut);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult SummaryPost(CartSummaryModel model)
    {
        var inquiryHeader = new InquiryHeaderDTO
        {
            AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            FullName = model.AppAppUser.FullName,
            PhoneNumber = model.AppAppUser.PhoneNumber,
            Email = model.AppAppUser.Email,
            Date = DateTime.Now
        };
        _repoInquiryHeader.Add(inquiryHeader);
        _repoInquiryHeader.SaveChanges();

        foreach (var item in model.ItemCollection!)
            _repoInquiryDetail.Add(new InquiryDetailDTO
            {
                InquiryHeaderId = inquiryHeader.Id,
                ProductId = item.Product!.Id,
                SqFt = item.SqFt
            });

        _repoInquiryDetail.SaveChanges();
        HttpContext.Session.Clear();

        TempData[Const.Success] = "Order created successfully";
        return RedirectToAction("Index", "Home");
    }

    #endregion Summary

    #region Update

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Update(IList<CartIndexModel> modelIn)
    {
        var sessionCart = new List<ShoppingCart>();
        sessionCart.AddRange(modelIn.Select(item => new ShoppingCart
        {
            ProductId = item.Product!.Id,
            SqFt = item.SqFt is > 0 and <= 10_000 ? item.SqFt : 1
        }));
        HttpContext.Session.Set(Const.SessionCartList, sessionCart);

        if (modelIn.Any() is false)
            TempData[Const.Error] = "Error while cart update";
        else TempData[Const.Success] = "Cart updated successfully";

        return RedirectToAction(nameof(Index));
    }

    #endregion Update

    #region ControllerHelpers

    private List<ShoppingCart>? GetSessionCart =>
        HttpContext.Session.Get<List<ShoppingCart>>(Const.SessionCartList);

    #endregion ControllerHelpers
}