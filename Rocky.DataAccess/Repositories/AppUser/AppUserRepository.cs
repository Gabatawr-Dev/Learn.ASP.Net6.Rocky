using Rocky.DataAccess.Contexts;
using Rocky.Models;

namespace Rocky.DataAccess.Repositories.AppUser;

public class AppUserRepository : Repository<AppUserDTO>, IAppUserRepository
{
    public AppUserRepository(ApplicationDbContext context) : base(context)
    {
    }
}