using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DataAccess.Repositories.Product;
using Rocky.Models.Product;
using Rocky.Utility;

namespace Rocky.Server.Controllers;

[Authorize(Roles = Const.AdminRole)]
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductRepository _repoProduct;
    private readonly IWebHostEnvironment _env;

    public ProductController(
        ILogger<ProductController> logger,
        IProductRepository repoProduct,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _repoProduct = repoProduct;
        _env = env;
    }

    #region Index

    public IActionResult Index()
    {
        var model = _repoProduct
            .GetAll(include: nameof(ProductDTO.Category));

        return View(model);
    }

    #endregion Index

    #region Upsert

    public IActionResult Upsert(int? id)
    {
        var model = new ProductUpsertModel
        {
            Product = id != null
                ? _repoProduct.Find(id)
                : new ProductDTO(),
        };
        if (model.Product == null)
            return NotFound();

        model.CategoryList = _repoProduct.GetCategoryDropDown();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductUpsertModel model)
    {
        if (ModelState.IsValid is false)
        {
            model.CategoryList ??= _repoProduct.GetCategoryDropDown();
            TempData[Const.Error] = "Validation error";
            return View(model);
        }

        var isUpdate = model.Product!.Id != 0;
        var file = HttpContext.Request.Form.Files
            .FirstOrDefault();

        if (isUpdate)
        {
            var dbProduct = _repoProduct
                .FirstOrDefault(p => p.Id == model.Product.Id, isTracking: false);
            if (dbProduct == null)
                return NotFound();

            if (file != null)
            {
                RemoveImage(dbProduct.Image!);
                UpsertImage(model.Product, file);
            }
            else model.Product.Image = dbProduct.Image;

            _repoProduct.Update(model.Product);
            TempData[Const.Success] = "Product edited successfully";
        }
        else
        {
            UpsertImage(model.Product, file!);
            _repoProduct.Add(model.Product);
            TempData[Const.Success] = "Product created successfully";
        }
        _repoProduct.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #region UpsertHelpers

    private void UpsertImage(ProductDTO product, IFormFile file)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        product.Image = fileName;

        var upload = string.Concat(_env.WebRootPath, Const.ProductImages, fileName);
        using var fs = new FileStream(upload, FileMode.Create);
        file.CopyTo(fs);
    }

    #endregion UpsertHelpers

    #endregion Upsert

    #region Delete

    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var model = _repoProduct
            .FirstOrDefault(p => p.Id == id, include: nameof(ProductDTO.Category));
        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost, ActionName(nameof(Delete)), ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var model = _repoProduct.Find(id);
        if (model == null)
            return NotFound();

        RemoveImage(model.Image!);
        _repoProduct.Remove(model);
        _repoProduct.SaveChanges();

        TempData[Const.Success] = "Product deleted successfully";
        return RedirectToAction(nameof(Index));
    }

    #endregion Delete

    #region ControllerHelpers

    private void RemoveImage(string fileName)
    {
        var oldImage = string.Concat(_env.WebRootPath, Const.ProductImages, fileName);
        if (System.IO.File.Exists(oldImage))
            System.IO.File.Delete(oldImage);
    }

    #endregion ControllerHelpers
}

