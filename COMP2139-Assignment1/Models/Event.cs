using System.ComponentModel.DataAnnotations;

namespace COMP2139_Assignment1.Models;

public class Event
{
    public int EventId { get; set; }
    [Required]
    public string Title { get; set; }
    public Category? Category { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    [Required]
    public double TicketPrice { get; set; }
    [Required]
    public int AvailableTickets { get; set; }
}