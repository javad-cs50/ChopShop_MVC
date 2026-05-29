using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChopShop.Models;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }
    [Range(1,10,ErrorMessage ="Please Enter between 1-10")]
    public int Count { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    [ValidateNever]
    public IdentityUser User { get; set; }
}
