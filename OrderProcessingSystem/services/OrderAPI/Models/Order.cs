using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Models;

[Table("Order")]
public class Order //: IValidatableObject
{
    [Key]
    public Guid OrderId { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "CustomerId is required")]
    public Guid CustomerId { get; set; }
    
    [Required(ErrorMessage = "CustomerName is required")]
    [StringLength(100, ErrorMessage = "CustomerName cannot be longer than 100 characters.")]
    public string? CustomerName { get; set; }
    
    [Required(ErrorMessage = "OrderDate is required")]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }
    
    [Required(ErrorMessage = "TotalAmount is required")]
    [DataType(DataType.Currency)]
    public decimal TotalAmount { get; set; }

    // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    // {
    //     throw new NotImplementedException();
    // }
}