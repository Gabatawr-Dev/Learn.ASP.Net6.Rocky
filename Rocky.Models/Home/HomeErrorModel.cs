namespace Rocky.Models.Home;

public class HomeErrorModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => string.IsNullOrEmpty(RequestId) is false;
}