using System;
using System.Collections.Generic;
using System.Text;

namespace ChopShop.Models.ViewModels;

public class OrderVM
{
    public OrderHeader OrderHeader { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}
