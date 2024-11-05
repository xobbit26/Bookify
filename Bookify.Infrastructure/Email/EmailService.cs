using Bookify.Application.Abstractions.Email;

namespace Bookify.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        //Simulation of successfully send message
        //TODO: implement the real sending in the future
        return Task.CompletedTask;
    }
}