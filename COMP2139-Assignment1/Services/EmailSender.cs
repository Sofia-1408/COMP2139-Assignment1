using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks; 

namespace COMP2139_Assignment1.Services
{
    // Plain email sender
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            // This will satisfy identity requirements
            return Task.CompletedTask;
        }
    }
}