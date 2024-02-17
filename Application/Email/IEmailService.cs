using Application.Core;
using Application.Data;

namespace Application.Email
{
    public interface IEmailService
    {
        public Task<Result<string>> Send(string recipientEmail, string fileLink);
    }
}
