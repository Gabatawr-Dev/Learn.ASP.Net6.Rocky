using Rocky.DataAccess.Contexts;
using Rocky.Models.Category;

namespace Rocky.DataAccess.Repositories.Category;

public class CategoryRepository : Repository<CategoryDTO>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}