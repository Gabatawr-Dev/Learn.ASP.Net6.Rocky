using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models.Category;

public class CategoryDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}