using Bookify.Application.Abstractions.Email;

namespace Bookify.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendEmailAsync(Domain.Users.Email recipient, string subject, string body)
        {
            return Task.Run(() =>
            {
                // Simulate sending an email
                Console.WriteLine($"Sending email to: {recipient.Value}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Body: {body}");
            });
        }
    }
}
