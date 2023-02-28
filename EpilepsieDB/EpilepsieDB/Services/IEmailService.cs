using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{
    public interface IEmailService : IEmailSender
    {
        Task SendConfirmationMail(string email, string confirmationLink);
    }
}
