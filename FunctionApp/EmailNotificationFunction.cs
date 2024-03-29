using Application.Core;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Functions.Worker;
using Application.Email;

namespace FunctionApp
{
    public class EmailNotificationFunction
    {
        private readonly EmailService _emailService;

        public EmailNotificationFunction(IOptions<SmtpData> options)
        {
            SmtpSecutiry smtpSecutiry = new SmtpSecutiry()
            {
                SMTPServer = options.Value.SMTPServer,
                Port = options.Value.Port,
                Login = options.Value.Login,
                Password = options.Value.Password,
            };
            _emailService = new EmailService(Options.Create(smtpSecutiry));
        }

        [Function(nameof(EmailNotificationFunction))]
        public async Task<bool> Run([BlobTrigger("tesktask/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name, 
            IDictionary<string, string> metaData)
        {
            var email = metaData["email"];
            var fileLink = metaData["fileLink"];

            await Task.Delay(5000);
            var result = await _emailService.SendAsync(email, fileLink);

            return result.IsSuccess;
        }
    }
}
