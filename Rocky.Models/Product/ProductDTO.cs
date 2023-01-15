using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rocky.Models.Category;

namespace Rocky.Models.Product;

public class ProductDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public double Price { get; set; }

    public string? Image { get; set; }

    [Display(Name = "Category Type")]
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public virtual CategoryDTO? Category { get; set; }

    [NotMapped]
    public string ShortDescription => string.IsNullOrWhiteSpace(Description)
        ? string.Empty
        : Description.Length > 126
            ? Description[..126] + "..."
            : Description;
}
