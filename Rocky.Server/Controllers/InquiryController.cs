using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DataAccess.Repositories.Inquiry;
using Rocky.Models;
using Rocky.Models.Inquiry;
using Rocky.Utility;
using Const = Rocky.Utility.Const;

namespace Rocky.Server.Controllers;

[Authorize(Roles = Const.AdminRole)]
public class InquiryController : Controller
{
    private readonly IInquiryHeaderRepository _repoInquiryHeader;
    private readonly IInquiryDetailRepository _repoInquiryDetail;

    public InquiryController(IInquiryHeaderRepository repoInquiryHeader, IInquiryDetailRepository repoInquiryDetail)
    {
        _repoInquiryHeader = repoInquiryHeader;
        _repoInquiryDetail = repoInquiryDetail;
    }

    #region Api

    public IActionResult GetInquiryList() =>
        Json(new { data = _repoInquiryHeader.GetAll() });

    #endregion Api

    #region Index

    public IActionResult Index() => View();

    #endregion Index

    #region Details

    public IActionResult Details(int id)
    {
        var header = _repoInquiryHeader.Find(id);
        if (header == null) return NotFound();

        return View(new InquiryDetailsModel
        {
            InquiryHeader = header,
            InquiryDetails = GetDetailByHeaderId(id)
        });
    }

    [HttpPost] [ValidateAntiForgeryToken]
    public IActionResult Details(InquiryDetailsModel model)
    {
        if (ModelState.IsValid is false)
        {
            TempData[Const.Error] = "Error converting to cart";
            return View(model);
        }

        model.InquiryDetails = GetDetailByHeaderId(model.InquiryHeader!.Id);

        HttpContext.Session.Clear();
        HttpContext.Session.Set(Const.SessionInquiryId, model.InquiryHeader.Id);
        HttpContext.Session.Set(Const.SessionCartList,
            model.InquiryDetails
                .Select(m => new ShoppingCart
                {
                    ProductId = m.ProductId,
                    SqFt = m.SqFt,
                })
                .ToList());

        TempData[Const.Success] = "Order converted to cart successfully";
        return RedirectToAction("Index", "Cart");
    }

    #region DetailsHerpers

    IEnumerable<InquiryDetailDTO> GetDetailByHeaderId(int id) =>
        _repoInquiryDetail.GetAll(m => m.InquiryHeaderId == id,
            include: nameof(InquiryDetailDTO.Product));

    #endregion DetailsHerpers

    #endregion Details

    #region Delete

    [HttpPost] [ValidateAntiForgeryToken]
    public IActionResult Delete(InquiryDetailsModel model)
    {
        var header = (_repoInquiryHeader.Find(model.InquiryHeader!.Id));
        if (header == null) return NotFound();

        model.InquiryDetails = GetDetailByHeaderId(model.InquiryHeader!.Id);
        _repoInquiryHeader.Remove(header);
        _repoInquiryDetail.RemoveRange(model.InquiryDetails);

        _repoInquiryHeader.SaveChanges();
        TempData[Const.Success] = "Order removed successfully";
        return RedirectToAction(nameof(Index));
    }

    #endregion Delete
}