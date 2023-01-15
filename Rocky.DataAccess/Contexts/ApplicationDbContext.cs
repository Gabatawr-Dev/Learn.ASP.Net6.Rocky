using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rocky.Models;
using Rocky.Models.Category;
using Rocky.Models.Inquiry;
using Rocky.Models.Product;

namespace Rocky.DataAccess.Contexts;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<AppUserDTO> AppUsers { get; set; } = null!;

    public DbSet<CategoryDTO> Category { get; set; } = null!;
    public DbSet<ProductDTO> Product { get; set; } = null!;

    public DbSet<InquiryHeaderDTO> InquiryHeader { get; set; } = null!;
    public DbSet<InquiryDetailDTO> InquiryDetail { get; set; } = null!;
}