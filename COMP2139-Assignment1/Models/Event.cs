using System.ComponentModel.DataAnnotations;

namespace COMP2139_Assignment1.Models;

public class Event //Event model
{
    public int EventId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    [Required]
    public double TicketPrice { get; set; }
    [Required]
    public int AvailableTickets { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public List<Purchase>? Purchases { get; set; }
    
}