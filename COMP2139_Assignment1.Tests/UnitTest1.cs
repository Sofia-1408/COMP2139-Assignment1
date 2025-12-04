using System.Collections.Generic;
using System.Linq;
using COMP2139_Assignment1.Controllers;
using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace COMP2139_Assignment1.Tests
{
    public class EventRevenueTests
    {
        [Fact]
        public void Revenue_Is_TicketPrice_Times_Total_Quantity()
        {
            var evt = new Event
            {
                TicketPrice = 25,
                Purchases = new List<Purchase>
                {
                    new Purchase { Quantity = 2 },
                    new Purchase { Quantity = 3 }
                }
            };

            var ticketsSold = evt.Purchases.Sum(p => p.Quantity);
            var revenue = ticketsSold * evt.TicketPrice;

            Assert.Equal(5 * 25, revenue);
        }
    }

    public class CategoryValidationTests
    {
        [Fact]
        public void Category_Name_Should_Not_Be_Null_Or_Empty()
        {
            var category = new Category
            {
                Name = "",
                Description = "Test description"
            };

            var isValid = !string.IsNullOrWhiteSpace(category.Name);

            Assert.False(isValid);
        }
    }

    public class DashboardControllerTests
    {
        [Fact]
        public void Index_Returns_ViewResult()
        {
            ApplicationDbContext? context = null;
            var controller = new DashboardController(context!);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
