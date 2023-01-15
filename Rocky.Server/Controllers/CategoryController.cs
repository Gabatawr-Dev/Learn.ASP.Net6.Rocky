using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.DataAccess.Repositories.Category;
using Rocky.Models.Category;
using Rocky.Utility;

namespace Rocky.Server.Controllers;

[Authorize(Roles = Const.AdminRole)]
public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryRepository _repoCategory;

    public CategoryController(ILogger<CategoryController> logger, ICategoryRepository repoCategory)
    {
        _logger = logger;
        _repoCategory = repoCategory;
    }

    #region Index

    public IActionResult Index()
    {
        var items = _repoCategory.GetAll();
        return View(items);
    }

    #endregion Index

    #region Create

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(CategoryDTO model)
    {
        if (ModelState.IsValid is false)
            return View(model);

        _repoCategory.Add(model);
        _repoCategory.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion Create

    #region Edit

    public IActionResult Edit(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var model = _repoCategory.Find(id);
        if (model == null)
            return NotFound();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(CategoryDTO model)
    {
        if (ModelState.IsValid is false)
            return View(model);

        _repoCategory.Update(model);
        _repoCategory.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion Edit

    #region Delete

    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var model = _repoCategory.Find(id);
        if (model == null)
            return NotFound();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        if (id is null or 0)
            return NotFound();

        var model = _repoCategory.Find(id);
        if (model == null)
            return NotFound();

        _repoCategory.Remove(model);
        _repoCategory.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion Delete
}

