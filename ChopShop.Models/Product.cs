using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChopShop.Models;
public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    [Range(1, 1000)]
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }
    [ValidateNever]
    public string ImageUrl { get; set; } = string.Empty;


}
