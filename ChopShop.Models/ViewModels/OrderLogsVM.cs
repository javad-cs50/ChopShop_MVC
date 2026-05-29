namespace ChopShop.Models.ViewModels;

public class OrderLogsVM
{
    public IEnumerable<OrderHeader> ListOfOrders { get; set; }
    public int CountOfOrders { get; set; }
    public int CountOfSelledItems { get; set; }
    public double TotalFee { get; set; }
    public OrderHeader MostValueOrder = null;
    public List<TopSelledProductVM> TopSelledProducts { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}
