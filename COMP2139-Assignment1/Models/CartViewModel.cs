namespace COMP2139_Assignment1.Models;

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();
    public double Total => Items.Sum(i => i.TicketPrice * i.Quantity);
}