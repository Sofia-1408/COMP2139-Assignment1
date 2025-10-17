using COMP2139_Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Event> Events { get; set; } //Adding DbSet for events categories and purchases
    public DbSet<Category> Categories { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Event>() 
            .HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Purchase>()
            .HasOne(p => p.Event)
            .WithMany(e => e.Purchases)
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Cascade); 
        //Seed Data
        // Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Music", Description = "Concerts and live music events." },
            new Category { CategoryId = 2, Name = "Sports", Description = "Sports games and tournaments." },
            new Category { CategoryId = 3, Name = "Theatre", Description = "Plays and live performances." }
            ); 
        // Events
        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                EventId = 1,
                Title = "Summer Beats Festival",
                Date = DateTime.SpecifyKind(new DateTime(2025, 8, 15), DateTimeKind.Utc) ,
                TicketPrice = 75.00,
                AvailableTickets = 250,
                CategoryId = 1
            },
            new Event
            {
                EventId = 2,
                Title = "City Marathon 2025",
                Date = DateTime.SpecifyKind(new DateTime(2025, 9, 10), DateTimeKind.Utc),
                TicketPrice = 30.00,
                AvailableTickets = 500,
                CategoryId = 2
            },
            new Event 
            {
                EventId = 3,
                Title = "Shakespeare in the Park",
                Date = DateTime.SpecifyKind(new DateTime(2025, 7, 22), DateTimeKind.Utc),
                TicketPrice = 40.00,
                AvailableTickets = 150,
                CategoryId = 3
            });

            // Purchases
        modelBuilder.Entity<Purchase>().HasData(
            new Purchase
            {
                PurchaseId = 1,
                PurchaseDate = DateTime.SpecifyKind(new DateTime(2025, 6, 12), DateTimeKind.Utc),
                TotalCost = 150.00,
                GuestContactInfo = "Name: Alice Johnson, Email: alice@example.com",
                EventId = 1
            },
            new Purchase
            {
                PurchaseId = 2, 
                PurchaseDate = DateTime.SpecifyKind(new DateTime(2025, 6, 12), DateTimeKind.Utc),
                TotalCost = 60.00,
                GuestContactInfo = "Name: Bob Smith, Email: bob.example.com",
                EventId = 2
            },
            new Purchase
            {
                PurchaseId = 3,
                PurchaseDate = DateTime.SpecifyKind(new DateTime(2025, 6, 15), DateTimeKind.Utc),
                TotalCost = 80.00,
                GuestContactInfo = "Name: Carol Lee, Email: carol.example.com",
                EventId = 3
            }); 
    } 
}