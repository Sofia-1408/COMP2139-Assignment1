namespace COMP2139_Assignment1.Models;

public class CartItem
{
    public int EventId { get; set; }
    public string Title { get; set; }
    public double TicketPrice { get; set; }
    public int Quantity { get; set; }
    public int AvailableTickets { get; set; }
}