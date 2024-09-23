using System.Threading.Tasks;

namespace LibraryAPI.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
