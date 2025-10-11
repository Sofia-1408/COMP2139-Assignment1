using System.ComponentModel.DataAnnotations;

namespace COMP2139_Assignment1.Models;

public class Category
{
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}