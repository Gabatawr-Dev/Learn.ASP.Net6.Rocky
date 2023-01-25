namespace Rocky.Models.Inquiry;

public class InquiryDetailsModel
{
    public InquiryHeaderDTO? InquiryHeader { get; set; }
    public IEnumerable<InquiryDetailDTO>? InquiryDetails { get; set; }
}