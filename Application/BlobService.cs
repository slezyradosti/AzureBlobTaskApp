using Microsoft.Extensions.Options;

namespace Application
{
    public class BlobService : IBlobService
    {
        private readonly IOptions<BlobSecurity> _configuration;

        public BlobService(IOptions<BlobSecurity> options)
        {
            _configuration = options;
        }

    }
}
