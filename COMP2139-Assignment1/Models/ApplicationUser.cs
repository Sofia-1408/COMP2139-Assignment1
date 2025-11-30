using Microsoft.AspNetCore.Identity;

namespace COMP2139_Assignment1.Models

{
    // represents a user in our system - extends IdentityUser so we can add our own fields

    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}    