using System.ComponentModel.DataAnnotations;

namespace COMP2139_Assignment1.Models;

public class Category //Category model, nothing unlike what we did in labs
{
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    
    public List<Event>? Events { get; set; }
}