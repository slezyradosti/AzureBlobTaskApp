
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FunctionApplication
{
    public class EmailNotificationFunction
    {
        //private readonly EmailService _emailService;

        public EmailNotificationFunction()
        {
            //var configuration = new ConfigurationBuilder()
            //.<EmailNotificationFunction>()
            //.Build();

            //SmtpSecutiry smtpSecurity = new SmtpSecutiry();
            //configuration.GetSection("SmtpSecurity").Bind(smtpSecurity);

            //_emailService = new EmailService(Options.Create(smtpSecurity));
        }

        [FunctionName("EmailNotificationFunction")]
        public async Task Run([BlobTrigger("tesktask/{name}", Connection = "")]Stream myBlob, string name, ILogger log,
            IDictionary<string, string> metaData)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var email = metaData["email"];
            var fileLink = metaData["fileLink"];

            //await _emailService.SendAsync(email, fileLink);
        }
    }
}
