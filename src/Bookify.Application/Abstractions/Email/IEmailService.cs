using Bookify.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(Domain.Users.Email recipient, string subject, string body);
    }
}
