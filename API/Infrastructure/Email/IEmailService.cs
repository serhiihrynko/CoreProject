
using System.Threading.Tasks;

namespace API.Infrastructure.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
