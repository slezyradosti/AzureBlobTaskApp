using Application.Core;
using Application.Data;

namespace Application.Email
{
    public interface IEmailService
    {
        public Task<Result<string>> SendAsync(string recipientEmail, string fileLink);
    }
}
