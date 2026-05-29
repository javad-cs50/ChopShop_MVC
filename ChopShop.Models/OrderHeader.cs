using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChopShop.Models;

public class OrderHeader
{
    [Key]
    public int ID { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public IdentityUser User{ get; set; }
    public DateTime OrderDate { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    public decimal OrderTotal { get; set; }
    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
}
