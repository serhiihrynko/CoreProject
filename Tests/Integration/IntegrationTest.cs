using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace Tests.Integration
{
    public abstract class IntegrationTest
    {
        protected readonly HttpClient _client;

        public IntegrationTest()
        {
            WebApplicationFactory<API.Startup> appFactory = new WebApplicationFactory<API.Startup>();

            _client = appFactory.CreateClient();
        }
    }
}
