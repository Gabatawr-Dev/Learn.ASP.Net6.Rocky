using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DataAccess.Repositories.AppUser;
using Rocky.DataAccess.Repositories.Inquiry;
using Rocky.DataAccess.Repositories.Product;
using Rocky.Models;
using Rocky.Models.Cart;
using Rocky.Models.Inquiry;
using Rocky.Models.Product;
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
        var idCollection = GetSessionCart
            ?.Select(p => p.ProductId)
            .ToList();

        var model = idCollection == null || idCollection.Any() is false
            ? new List<ProductDTO>()
            : _repoProduct.GetAll(p => idCollection.Contains(p.Id))
                .ToList();

        return View(model);
    }

    [HttpPost, ActionName(nameof(Index)), ValidateAntiForgeryToken]
    public IActionResult IndexPost() => RedirectToAction(nameof(Summary));

    public IActionResult RemoveFromCart(int id)
    {
        var sessionCart = GetSessionCart;
        var product = sessionCart
            ?.FirstOrDefault(p => p.ProductId == id);

        if (product != null)
        {
            sessionCart!.Remove(product);
            HttpContext.Session.Set(Const.SessionCart, sessionCart);
            if (sessionCart.Any() is false)
                return RedirectToAction("Index", "Home");
        }
        return RedirectToAction(nameof(Index));
    }

    #endregion Index

    #region Summary

    public IActionResult Summary()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var idCollection = GetSessionCart
            ?.Select(p => p.ProductId)
            .ToList();
        
        var model = new CartSummaryModel
        {
            AppAppUser = _repoAppUser.Find(userId) 
                         ?? new AppUserDTO(),
            Products = idCollection == null || idCollection.Any() is false
                ? new List<ProductDTO>()
                : _repoProduct.GetAll(p => idCollection.Contains(p.Id))
                    .ToList()
    };
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Summary(CartSummaryModel model)
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

        foreach (var product in model.Products!)
            _repoInquiryDetail.Add(new InquiryDetailDTO
            {
                InquiryHeaderId = inquiryHeader.Id,
                ProductId = product.Id,
            });
        _repoInquiryDetail.SaveChanges();

        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    #endregion Summary

    #region ControllerHelpers

    private List<ShoppingCart>? GetSessionCart =>
        HttpContext.Session.Get<List<ShoppingCart>>(Const.SessionCart);

    #endregion ControllerHelpers
}