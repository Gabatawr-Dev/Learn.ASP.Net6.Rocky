using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.Models.Inquiry;

public class InquiryHeaderDTO
{
    [Key]
    public int Id { get; set; }

    public string? AppUserId { get; set; }

    [ForeignKey(nameof(AppUserId))]
    public AppUserDTO? AppUser { get; set; }

    public DateTime Date { get; set; }

    [Required]
    public string? FullName { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? Email { get; set; }
}