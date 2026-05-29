using System;
using System.Collections.Generic;
using System.Text;

namespace ChopShop.Models.ViewModels;

public class ShoppingCartVM
{
    public List<ShoppingCart> ShoppingCartList { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
