using Rocky.DataAccess.Contexts;
using Rocky.Models.Inquiry;

namespace Rocky.DataAccess.Repositories.Inquiry;

public class InquiryHeaderRepository : Repository<InquiryHeaderDTO>, IInquiryHeaderRepository
{
    public InquiryHeaderRepository(ApplicationDbContext context) : base(context)
    {
    }
}