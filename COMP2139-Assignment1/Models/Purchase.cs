using System.ComponentModel.DataAnnotations;

namespace COMP2139_Assignment1.Models;

public class Purchase
{
    [Required]
    public int PurchaseId { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }
    [Required]
    public double TotalCost { get; set; }
    [Required]
    public string GuestContactInfo { get; set; }
}