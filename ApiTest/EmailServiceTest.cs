using Application.Core;
using Application.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ApiTest
{
    public class EmailServiceTest
    {
        private readonly IEmailService _emailService;

        public EmailServiceTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EmailServiceTest>()
            .Build();

            SmtpSecutiry smtpSecurity = new SmtpSecutiry();
            configuration.GetSection("SmtpSecurity").Bind(smtpSecurity);

            _emailService = new EmailService(Options.Create(smtpSecurity));
        }

        [Fact]
        public async Task SendMailsFail()
        {
            var result1 = await _emailService.SendAsync(null, null);
            Assert.False(result1.IsSuccess);

            var result2 = await _emailService.SendAsync(string.Empty, string.Empty);
            Assert.False(result2.IsSuccess);

            var result3 = await _emailService.SendAsync("sss", "sss");
            Assert.False(result3.IsSuccess);
        }

        [Fact]
        public async Task SendMailsSuccess()
        {
            var result1 = await _emailService.SendAsync("test@testmail.com", "testlink");
            Assert.True(result1.IsSuccess);

            var result2 = await _emailService.SendAsync("t@mail.com", "link2");
            Assert.True(result2.IsSuccess);
        }
    }
}
