using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rocky.Models.Product;

namespace Rocky.Models.Inquiry;

public class InquiryDetailDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InquiryHeaderId { get; set; }

    [ForeignKey(nameof(InquiryHeaderId))]
    public InquiryHeaderDTO? InquiryHeader { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public ProductDTO? Product { get; set; }

    [Required]
    [Range(1, 10_000)]
    public uint SqFt { get; set;}
}