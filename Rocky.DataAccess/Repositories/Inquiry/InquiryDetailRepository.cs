using Rocky.DataAccess.Contexts;
using Rocky.Models.Inquiry;

namespace Rocky.DataAccess.Repositories.Inquiry;

public class InquiryDetailRepository : Repository<InquiryDetailDTO>, IInquiryDetailRepository
{
    public InquiryDetailRepository(ApplicationDbContext context) : base(context)
    {
    }
}