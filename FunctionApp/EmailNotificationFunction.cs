using Application.Email;
using Application.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Functions.Worker;

namespace FunctionApp
{
    public class EmailNotificationFunction
    {
        private readonly EmailService _emailService;

        public EmailNotificationFunction()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EmailNotificationFunction>()
            .Build();

            SmtpSecutiry smtpSecurity = new SmtpSecutiry();
            //configuration.GetSection("SmtpSecurity").Bind(smtpSecurity);
            configuration.GetSection("SmtpOutlookSecurity").Bind(smtpSecurity);

            _emailService = new EmailService(Options.Create(smtpSecurity));
        }

        [Function(nameof(EmailNotificationFunction))]
        public async Task<bool> Run([BlobTrigger("tesktask/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name, 
            IDictionary<string, string> metaData)
        {
            var email = metaData["email"];
            var fileLink = metaData["fileLink"];

            var result = await _emailService.SendAsync(email, fileLink);

            return result.IsSuccess;
        }
    }
}
