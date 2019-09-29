using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace Tests.Integration
{
    public abstract class IntegrationTest
    {
        protected readonly HttpClient Client;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<API.Startup>();

            Client = appFactory.CreateClient();
        }
    }
}
