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

            var result2 = await _emailService.SendAsync(string.Empty, null);
            Assert.False(result2.IsSuccess);

            var result3 = await _emailService.SendAsync(null, string.Empty);
            Assert.False(result3.IsSuccess);

            var result4 = await _emailService.SendAsync(string.Empty, string.Empty);
            Assert.False(result4.IsSuccess);

            var result5 = await _emailService.SendAsync("sss", "sss");
            Assert.False(result5.IsSuccess);

            var result6 = await _emailService.SendAsync("sss", null);
            Assert.False(result6.IsSuccess);

            var result7 = await _emailService.SendAsync(null, "sss");
            Assert.False(result7.IsSuccess);

            var result8 = await _emailService.SendAsync("sss", string.Empty);
            Assert.False(result8.IsSuccess);

            var result9 = await _emailService.SendAsync(string.Empty, "sss");
            Assert.False(result9.IsSuccess);
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
