using System;
using System.Collections.Generic;
using System.Text;

namespace ChopShop.Models.ViewModels;

public class TopSelledProductVM
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string ImgUrl { get; set; }
    public int TotalCount { get; set; }

}
