using FakeItEasy;
using FunctionApp;

namespace ApiTest
{
    public class BlobAzureFunctionTest
    {
        [Theory]
        [InlineData("tes", "link", "name")]
        [InlineData("tes@c.com", null, "name")]
        [InlineData(null, null, null)]
        public async Task FunctionRunFail(string email, string fileLink, string name)
        {
            EmailNotificationFunction emailNotificationFunction = new EmailNotificationFunction();

            var stream = A.Fake<Stream>();
            Dictionary<string, string> meta = new Dictionary<string, string>();
            meta.Add("email", email);
            meta.Add("fileLink", fileLink);

            bool funcResult = true;
            try
            {
                funcResult = await emailNotificationFunction.Run(stream, name, meta);
            }
            catch (Exception ex)
            {
                funcResult = false;
            }
            finally
            {
                Assert.False(funcResult);
            }
        }

        [Theory]
        [InlineData("niyosev242@huizk.com", "link1", "n")]
        [InlineData("receb56544@gexige.com", "link2", "")]
        public async Task FunctionRunSuccess(string email, string fileLink, string name)
        {
            EmailNotificationFunction emailNotificationFunction = new EmailNotificationFunction();

            var stream = A.Fake<Stream>();
            Dictionary<string, string> meta = new Dictionary<string, string>() 
            { 
                { "email", email},
                { "fileLink", fileLink}
            };

            bool funcResult = true;
            try
            {
                funcResult = await emailNotificationFunction.Run(stream, name, meta);
            }
            catch (Exception ex)
            {
                funcResult = false;
            }
            finally
            {
                Assert.True(funcResult);
            }
        }
    }
}
