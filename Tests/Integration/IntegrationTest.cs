using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;

namespace Tests.Integration
{
    public abstract class IntegrationTest : IClassFixture<WebApplicationFactory<API.Startup>>
    {
        protected readonly HttpClient _httpclient;
        protected readonly WebApplicationFactory<API.Startup> _webAppFactory;

        protected IntegrationTest(WebApplicationFactory<API.Startup> factory)
        {
            _webAppFactory = factory;
            _httpclient = _webAppFactory.CreateClient();
        }
    }
}
