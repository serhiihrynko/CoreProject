using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Integration
{
    public class StatusControllerTests : IntegrationTest
    {
        public StatusControllerTests(WebApplicationFactory<API.Startup> factory) 
            : base(factory) 
        { 
        }


        [Fact]
        public async Task GetStatusReturnsUptime()
        {
            // Arrange

            // Act
            var response = await _httpclient.GetAsync("/status");
            string message = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.False(string.IsNullOrEmpty(message));
        }
    }
}
